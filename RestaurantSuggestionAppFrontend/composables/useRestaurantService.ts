export const useRestaurantService = () => {
  const api = useApi()

  return {
    getNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      return api.get(`/api/Restaurant/nearby?latitude=${latitude}&longitude=${longitude}&radiusInKm=${radiusInKm}`)
    }
  }
}
