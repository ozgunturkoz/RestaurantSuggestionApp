import { defineStore } from 'pinia'

export const useRestaurantStore = defineStore('restaurant', {
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
      restaurants: [] as any[],
      radius: 5,
      isLoading: false,
      userLocation: savedLocation
    }
  },

  actions: {
    async fetchNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      const restaurantService = useRestaurantService()
      this.isLoading = true
      this.radius = radiusInKm
      this.userLocation = { lat: latitude, lng: longitude }

      try {
        localStorage.setItem('saved_user_lat', latitude.toString())
        localStorage.setItem('saved_user_lng', longitude.toString())
      } catch (e) {}

      try {
        const response = await restaurantService.getNearbyRestaurants(latitude, longitude, radiusInKm) as any
        
        const list = response?.data || response

        if (list && Array.isArray(list)) {
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
    },

    resetState() {
      this.restaurants = []
      this.isLoading = false
    }
  }
})