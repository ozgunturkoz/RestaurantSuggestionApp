<template>
  <!-- =========================================================================================
       İNTERAKTİF HARİTA BİLEŞENİ (LEAFLET / OPENSTREETMAP) (KODLAMA BİLMEYENLER İÇİN ANLATIM)
       Bu bileşen, web sayfasının ortasında Leaflet kütüphanesini kullanarak interaktif bir
       dünya haritası çizer.
       1. Kullanıcının haritadaki yerini mavi bir nokta ile işaretler.
       2. Seçilen yarıçapı (örn: 5 KM) şık bir daire olarak çizer.
       3. Yakındaki restoranları harita üzerine sarı/yeşil pinlerle yerleştirir.
       4. Kullanıcı haritada herhangi bir yere tıkladığında orayı yeni merkez kabul eder.
       ========================================================================================= -->
  <div ref="mapContainer" style="height: 100%; width: 100%; z-index: 0;"></div>
</template>

<script setup>
// =========================================================================================
// HARİTA KODLARI (JAVASCRIPT / LEAFLET)
// =========================================================================================
import { ref, onMounted, watch, nextTick, onBeforeUnmount } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'

// Üst bileşenden gelen seçili restoran parametresi
const props = defineProps({
  selectedRestaurantId: {
    type: Number,
    default: null
  }
})

// Haritada bir restorana tıklandığında dışarıya haber veren olay
const emit = defineEmits(['select-restaurant'])

// Çoklu dil ve restoran deposu
const { t, locale } = useI18n()
const store = useRestaurantStore()

// Haritanın çizileceği HTML kutusu referansı
const mapContainer = ref(null)

const isLocationLoaded = ref(false)
const userLocation = ref(null)
let resizeObserver = null
let mapInstance = null
let handleResize = null
const markerMap = new Map()

// -----------------------------------------------------------------------------------------
// Haritadaki Restoran İğneleri (Marker Icon) İçin Özel HTML Tasarımı Üreten Fonksiyon
// -----------------------------------------------------------------------------------------
const createCustomIcon = (color, isSelected = false) => {
  const size = isSelected ? 22 : 16;
  const borderWidth = isSelected ? '3px' : '2px';
  const zIndex = isSelected ? 1000 : 1;
  const shadow = isSelected ? '0 0 12px #E0A458, 0 2px 6px rgba(0,0,0,0.5)' : '0 2px 4px rgba(0,0,0,0.3)';

  return L.divIcon({
    html: `
      <div style="
        background-color: ${color};
        width: ${size}px;
        height: ${size}px;
        border-radius: 50% 50% 50% 0;
        transform: rotate(-45deg);
        border: ${borderWidth} solid white;
        box-shadow: ${shadow};
        z-index: ${zIndex};
        transition: all 0.2s ease;
      "></div>
    `,
    className: 'custom-marker',
    iconSize: [size, size],
    iconAnchor: [size / 2, size],
    popupAnchor: [0, -size]
  });
};

// -----------------------------------------------------------------------------------------
// Haritanın Ekrana İlk Yüklendiği An (onMounted Lifecycle)
// -----------------------------------------------------------------------------------------
onMounted(async () => {
  await nextTick()
  if (!mapContainer.value) {
    return;
  }

  // Hafızada kayıtlı son konum yoksa İstanbul Kadıköy varsayılan alınır
  const savedLat = parseFloat(localStorage.getItem('saved_user_lat') || '40.9901')
  const savedLng = parseFloat(localStorage.getItem('saved_user_lng') || '29.0256')
  const map = L.map(mapContainer.value).setView([savedLat, savedLng], 13)
  mapInstance = map;

  // Koyu temalı modern harita katmanını (CARTO Dark Matter) yüklüyoruz
  L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png', {
    maxZoom: 19,
    subdomains: 'abcd',
    attribution: '&copy; OpenStreetMap &copy; CARTO'
  }).addTo(map)

  // Ekran boyutu değiştiğinde haritayı yeniden hesaplayan izleyici
  resizeObserver = new ResizeObserver(() => {
    map.invalidateSize();
  });
  resizeObserver.observe(mapContainer.value);

  setTimeout(() => {
    if (mapInstance) mapInstance.invalidateSize();
  }, 200);
  setTimeout(() => {
    if (mapInstance) mapInstance.invalidateSize();
  }, 600);

  const markerGroup = L.layerGroup().addTo(map);
  let boundaryCircle = null;

  // Seçili yarıçapı haritada daire olarak çizen fonksiyon
  const drawBoundaryCircle = (lat, lng, radiusInKm) => {
    if (boundaryCircle) {
      map.removeLayer(boundaryCircle);
    }
    boundaryCircle = L.circle([lat, lng], {
      color: '#E0A458',
      fillColor: '#E0A458',
      fillOpacity: 0.05,
      weight: 1,
      radius: radiusInKm * 1000
    }).addTo(map);
  };

  const formatDistance = (meters) => {
    if (meters === null || meters === undefined) return '';
    if (meters < 1000) return `${Math.round(meters)} m`;
    return `${(meters / 1000).toFixed(1)} km`;
  };

  const formatDuration = (seconds) => {
    if (seconds === null || seconds === undefined) return '';
    const unit = locale.value === 'en' ? 'min' : 'dk';
    return `${Math.round(seconds / 60)} ${unit}`;
  };

  // ---------------------------------------------------------------------------------------
  // Restoranları Haritada İğnelerle (Markers) Çizen Fonksiyon
  // ---------------------------------------------------------------------------------------
  const renderMarkers = () => {
    markerGroup.clearLayers();
    markerMap.clear();
    
    store.restaurants.forEach(rest => {
      if (rest.latitude && rest.longitude) {
        const isSelected = props.selectedRestaurantId === rest.id;
        const pinColor = isSelected ? '#8FBF7E' : '#E0A458';

        const walkInfo = rest.walkingDistanceInMeters !== null
          ? `<br>${t('map.walk')}: ${formatDistance(rest.walkingDistanceInMeters)} (${formatDuration(rest.walkingDurationInSeconds)})`
          : '';

        let scoreHtml = '';
        if (rest.recommendationScore !== null && rest.recommendationScore !== undefined) {
          let badgeColor = '#E08E58';
          let bg = 'rgba(220,53,69,0.12)';
          let border = 'rgba(220,53,69,0.25)';

          if (rest.recommendationScore >= 80) {
            badgeColor = '#8FBF7E';
            bg = 'rgba(40,167,69,0.15)';
            border = 'rgba(40,167,69,0.3)';
          } else if (rest.recommendationScore >= 50) {
            badgeColor = '#E0A458';
            bg = 'rgba(224,164,88,0.12)';
            border = 'rgba(224,164,88,0.25)';
          }

          scoreHtml = `
            <div style="
              margin-top: 8px; 
              padding: 6px 10px; 
              border-radius: 8px; 
              background: ${bg}; 
              border: 1px solid ${border}; 
              color: ${badgeColor}; 
              font-size: 11px; 
              font-weight: bold; 
              font-family: 'Manrope', sans-serif;
              line-height: 1.4;
            ">
              ${t('map.match')}: %${Math.round(rest.recommendationScore)}
              <div style="font-size: 9.5px; font-weight: 500; opacity: 0.85; margin-top: 2px;">
                ${t('map.locationScore')}: %${Math.round(rest.locationScore)} | ${t('map.budgetScore')}: %${Math.round(rest.budgetScore)}
              </div>
            </div>
          `;
        }

        const marker = L.marker([rest.latitude, rest.longitude], { icon: createCustomIcon(pinColor, isSelected) })
          .addTo(markerGroup)
          .bindPopup(`
            <b style="font-family: 'Manrope', sans-serif; font-size: 13px; color: #33291F;">${rest.name}</b><br>
            <span style="font-family: 'Manrope', sans-serif; font-size: 11px; color: #666;">${t('map.category')}: ${rest.category}</span><br>
            <span style="font-family: 'Manrope', sans-serif; font-size: 11px; color: #666;">${t('map.price')}: ${rest.averagePrice}₺</span>
            ${walkInfo}
            ${scoreHtml}
          `);

        marker.on('click', () => {
          emit('select-restaurant', rest.id);
        });

        markerMap.set(rest.id, { marker, rest });
      }
    });
  };

  // Haritayı seçilen restorana yumuşakça kaydıran (FlyTo) fonksiyon
  const focusOnRestaurant = (id) => {
    if (!id || !markerMap.has(id)) return;
    const { marker, rest } = markerMap.get(id);

    if (mapInstance && rest.latitude && rest.longitude) {
      mapInstance.flyTo([rest.latitude, rest.longitude], 15, { duration: 0.8 });
      marker.openPopup();
    }
  };

  let userMarkerLayer = null;

  // Kullanıcının konumunu haritada güncelleyen fonksiyon
  const updateUserLocation = (lat, lng, isUserAction = false) => {
    userLocation.value = { lat, lng };

    try {
      localStorage.setItem('saved_user_lat', lat.toString());
      localStorage.setItem('saved_user_lng', lng.toString());
    } catch {}
    
    if (userMarkerLayer) {
      map.removeLayer(userMarkerLayer);
    }

    userMarkerLayer = L.circle([lat, lng], {
      color: '#007bff',
      fillColor: '#007bff',
      fillOpacity: 0.8,
      radius: 20
    })
    .addTo(map)
    .bindPopup(`<b>${t('map.userMarker')}</b>`);

    drawBoundaryCircle(lat, lng, store.radius);

    store.fetchNearbyRestaurants(lat, lng, store.radius).then(() => {
      isLocationLoaded.value = true;
      renderMarkers();
    });
  };

  // Haritaya tıklandığında orayı yeni merkez yapan olay dinleyici
  map.on('click', (e) => {
    updateUserLocation(e.latlng.lat, e.latlng.lng, true);
  });

  const initializeWithLocation = (lat, lng) => {
    map.setView([lat, lng], 13);
    updateUserLocation(lat, lng, false);
  };

  const getSavedOrDefaultLat = () => parseFloat(localStorage.getItem('saved_user_lat') || '41.0082');
  const getSavedOrDefaultLng = () => parseFloat(localStorage.getItem('saved_user_lng') || '28.9784');

  // Tarayıcının GPS Geolocation desteği varsa gerçek konumu alır
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        initializeWithLocation(position.coords.latitude, position.coords.longitude);
      },
      () => {
        initializeWithLocation(getSavedOrDefaultLat(), getSavedOrDefaultLng());
      },
      { timeout: 8000, maximumAge: 30000, enableHighAccuracy: true }
    );
  } else {
    initializeWithLocation(getSavedOrDefaultLat(), getSavedOrDefaultLng());
  }

  // Yarıçap değiştiğinde haritayı tazeleyen izleyici
  watch(() => store.radius, (newRadius) => {
    if (userLocation.value) {
      drawBoundaryCircle(userLocation.value.lat, userLocation.value.lng, newRadius);
      store.fetchNearbyRestaurants(userLocation.value.lat, userLocation.value.lng, newRadius).then(() => {
        renderMarkers();
      });
    }
  });

  // Dil değiştiğinde haritadaki popup metinlerini tazeleyen izleyici
  watch(() => locale.value, () => {
    renderMarkers();
  });

  // Seçili restoran değiştiğinde oraya odaklanan izleyici
  watch(() => props.selectedRestaurantId, (newId) => {
    renderMarkers();
    if (newId) {
      focusOnRestaurant(newId);
    }
  });

  watch(() => store.restaurants, () => {
    renderMarkers();
  });

  handleResize = () => {
    if (mapInstance) {
      mapInstance.invalidateSize({ animate: false })
    }
  }

  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  if (resizeObserver) {
    resizeObserver.disconnect();
  }
  if (handleResize) {
    window.removeEventListener('resize', handleResize);
  }
})
</script>

<style>
.leaflet-container {
  height: 100% !important;
  width: 100% !important;
  background: #17120F !important;
  font-family: 'Manrope', sans-serif !important;
}

.leaflet-tile {
  max-width: none !important;
  max-height: none !important;
}

.leaflet-tile-container img {
  max-width: none !important;
  max-height: none !important;
}
</style>