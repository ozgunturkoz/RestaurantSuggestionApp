export const useRestaurantService = () => {
  const api = useApi()

  return {
    getAllRestaurants() {
      return api.get('/api/Restaurant')
    },
    getRestaurantById(id: number) {
      return api.get(`/api/Restaurant/${id}`)
    },
    getNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      return api.get(`/api/Restaurant/nearby?latitude=${latitude}&longitude=${longitude}&radiusInKm=${radiusInKm}`)
    } 
  }
}
