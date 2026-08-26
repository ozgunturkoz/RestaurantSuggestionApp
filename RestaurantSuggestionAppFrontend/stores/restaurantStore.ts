import { defineStore } from 'pinia'

// =========================================================================================
// BU STORE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Harita ve liste sayfalarında görüntülenen restoranları, kullanıcının seçtiği arama yarıçapını (1-10 km)
// ve kullanıcının son bilinen konumunu tarayıcı hafızasında saklar.
// =========================================================================================

export const useRestaurantStore = defineStore('restaurant', {
  // -------------------------------------------------------------------------------------
  // State: Hafızada tutulan değişkenler
  // -------------------------------------------------------------------------------------
  state: () => {
    let savedLocation: { lat: number, lng: number } | null = null
    if (typeof window !== 'undefined') {
      const lat = localStorage.getItem('saved_user_lat')
      const lng = localStorage.getItem('saved_user_lng')
      if (lat && lng) {
        savedLocation = { lat: parseFloat(lat), lng: parseFloat(lng) }
      }
    }

    return {
      // Ekranda listelenen restoranlar dizisi
      restaurants: [] as any[],

      // Seçili arama yarıçapı (KM)
      radius: 5,

      // Yükleniyor animasyonu durumu
      isLoading: false,

      // Kullanıcının harita koordinatları
      userLocation: savedLocation
    }
  },

  // -------------------------------------------------------------------------------------
  // Actions: Restoranları indirme ve sıfırlama eylemleri
  // -------------------------------------------------------------------------------------
  actions: {
    // Verilen koordinat ve yarıçapa göre en uygun restoranları sunucudan çeker
    async fetchNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      const restaurantService = useRestaurantService()
      this.isLoading = true
      this.radius = radiusInKm
      this.userLocation = { lat: latitude, lng: longitude }

      try {
        localStorage.setItem('saved_user_lat', latitude.toString())
        localStorage.setItem('saved_user_lng', longitude.toString())
      } catch {
        // Tarayıcı depolama erişiminde hata olursa sessizce devam edilir
      }

      try {
        const response = await restaurantService.getNearbyRestaurants(latitude, longitude, radiusInKm) as any
        const list = response?.data || response

        if (list && Array.isArray(list)) {
          this.restaurants = list
        } else {
          this.restaurants = []
        }
      } catch {
        this.restaurants = []
      } finally {
        this.isLoading = false
      }
    },

    // Çıkış yapıldığında veya durum sıfırlanırken listeyi temizler
    resetState() {
      this.restaurants = []
      this.isLoading = false
    }
  }
})