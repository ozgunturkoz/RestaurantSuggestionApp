<template>
  <div ref="mapContainer" style="height: 100%; width: 100%; z-index: 0;"></div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick, onBeforeUnmount } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import { useRestaurantStore } from '~/Stores/restaurantStore'

const { t, locale } = useI18n()
const store = useRestaurantStore()
const mapContainer = ref(null)

const isLocationLoaded = ref(false)
const userLocation = ref(null)
let resizeObserver = null

// Renkli Pin Üretici
const createCustomIcon = (color) => {
  return L.divIcon({
    html: `
      <div style="
        background-color: ${color};
        width: 16px;
        height: 16px;
        border-radius: 50% 50% 50% 0;
        transform: rotate(-45deg);
        border: 2px solid white;
        box-shadow: 0 2px 4px rgba(0,0,0,0.3);
      "></div>
    `,
    className: 'custom-marker',
    iconSize: [16, 16],
    iconAnchor: [8, 16],
    popupAnchor: [0, -16]
  });
};

onMounted(async () => {
  await nextTick()
  if (!mapContainer.value) {
    console.warn("Harita kapsayıcısı (DOM) henüz hazır değil.");
    return;
  }
  const map = L.map(mapContainer.value).setView([40.993462, 29.018418], 12)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '© OpenStreetMap'
  }).addTo(map)

  resizeObserver = new ResizeObserver(() => {
    map.invalidateSize();
  });
  resizeObserver.observe(mapContainer.value);

  const markerGroup = L.layerGroup().addTo(map);
  let boundaryCircle = null;

  const drawBoundaryCircle = (lat, lng, radiusInKm) => {
    if (boundaryCircle) {
      map.removeLayer(boundaryCircle);
    }
    boundaryCircle = L.circle([lat, lng], {
      color: '#6c757d',
      fillColor: '#6c757d',
      fillOpacity: 0.08,
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
    return `${Math.round(seconds / 60)} dk`;
  };

  const renderMarkers = () => {
    markerGroup.clearLayers();
    
    store.restaurants.forEach(rest => {
      if (rest.latitude && rest.longitude) {
        const pinColor = '#E0A458';

        const walkInfo = rest.walkingDistanceInMeters !== null
          ? `<br>🚶 ${t('map.walk')}: ${formatDistance(rest.walkingDistanceInMeters)} (${formatDuration(rest.walkingDurationInSeconds)})`
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
              🎯 ${t('map.match')}: %${Math.round(rest.recommendationScore)}
              <div style="font-size: 9.5px; font-weight: 500; opacity: 0.85; margin-top: 2px;">
                📍 ${t('map.locationScore')}: %${Math.round(rest.locationScore)} | ₺ ${t('map.budgetScore')}: %${Math.round(rest.budgetScore)}
              </div>
            </div>
          `;
        }

        L.marker([rest.latitude, rest.longitude], { icon: createCustomIcon(pinColor) })
          .addTo(markerGroup)
          .bindPopup(`
            <b style="font-family: 'Manrope', sans-serif; font-size: 13px; color: #33291F;">${rest.name}</b><br>
            <span style="font-family: 'Manrope', sans-serif; font-size: 11px; color: #666;">${t('map.category')}: ${rest.category}</span><br>
            <span style="font-family: 'Manrope', sans-serif; font-size: 11px; color: #666;">${t('map.price')}: ${rest.averagePrice}₺</span>
            ${walkInfo}
            ${scoreHtml}
          `);
      }
    });
  };

  const initializeWithLocation = (lat, lng, label) => {
    userLocation.value = { lat, lng };
    map.setView([lat, lng], 13);

    L.circle([lat, lng], {
      color: '#007bff',
      fillColor: '#007bff',
      fillOpacity: 0.8,
      radius: 15
    })
    .addTo(map)
    .bindPopup(`<b>${t('map.userMarker')}</b>`)
    .openPopup();

    drawBoundaryCircle(lat, lng, store.radius);

    store.fetchNearbyRestaurants(lat, lng, store.radius).then(() => {
      isLocationLoaded.value = true;
      renderMarkers();
    });
  };

  const DEFAULT_LAT = 40.9901;
  const DEFAULT_LNG = 29.0256;

  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        initializeWithLocation(position.coords.latitude, position.coords.longitude, t('map.userMarker'));
      },
      (error) => {
        console.warn("Gerçek konum alınamadı, Kadıköy varsayılan konumuna geçiliyor:", error);
        initializeWithLocation(DEFAULT_LAT, DEFAULT_LNG, t('map.userMarker'));
      },
      { timeout: 8000, maximumAge: 30000, enableHighAccuracy: true } // Masaüstü cihazlar için yeterli konum tespiti süresi veriyoruz
    );
  } else {
    initializeWithLocation(DEFAULT_LAT, DEFAULT_LNG, t('map.userMarker'));
  }

  watch(() => store.radius, (newRadius) => {
    if (userLocation.value) {
      drawBoundaryCircle(userLocation.value.lat, userLocation.value.lng, newRadius);
      store.fetchNearbyRestaurants(userLocation.value.lat, userLocation.value.lng, newRadius).then(() => {
        renderMarkers();
      });
    }
  });

  watch(() => locale.value, () => {
    renderMarkers();
  });

  store.$subscribe((mutation, state) => {
    renderMarkers();
  })
})

onBeforeUnmount(() => {
  if (resizeObserver) {
    resizeObserver.disconnect();
  }
})
</script>