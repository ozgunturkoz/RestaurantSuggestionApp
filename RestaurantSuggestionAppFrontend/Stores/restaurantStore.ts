import { defineStore } from 'pinia'

export const useRestaurantStore = defineStore('restaurant', {
  state: () => ({
    restaurants: [] as any[], // Yakındaki ve OSRM mesafeli restoranlar listesi
    radius: 5,                // Varsayılan yarıçap: 5 KM
    isLoading: false,
    userLocation: null as { lat: number, lng: number } | null // Kullanıcı konumunu saklamak için
  }),

  actions: {
    // Sadece yakındaki restoranları API'den çeken ana metodumuz
    async fetchNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      const restaurantService = useRestaurantService()
      this.isLoading = true
      this.radius = radiusInKm
      this.userLocation = { lat: latitude, lng: longitude }
      try {
        const response = await restaurantService.getNearbyRestaurants(latitude, longitude, radiusInKm) as any
        
        // Backend BaseResponse<List<RestaurantDistance>> objesi döndürdüğü için response.data kontrol ediliyor
        const list = response?.data || response

        if (list && Array.isArray(list)) {
          // Backend zaten OSRM mesafesine göre sıralı gönderdiği için doğrudan atıyoruz
          this.restaurants = list
        } else {
          this.restaurants = []
        }
      } catch (error) {
        console.error('Yakındaki restoranlar yüklenirken hata oluştu:', error)
        this.restaurants = []
      } finally {
        this.isLoading = false
      }
    }
  }
})