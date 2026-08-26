// =========================================================================================
// BU COMPOSABLE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Keşfet ve Harita sayfalarında restoranların backend'den çekilmesi için kullanılan API köprüsüdür.
// =========================================================================================

export const useRestaurantService = () => {
  const api = useApi()

  return {
    // Tüm restoranları getirir
    getAllRestaurants() {
      return api.get('/api/Restaurant')
    },

    // Tek bir restoranın detayını getirir
    getRestaurantById(id: number) {
      return api.get(`/api/Restaurant/${id}`)
    },

    // Kullanıcının enlemi, boylamı ve arama yarıçapına (km) göre akıllı öneri listesini çeker
    getNearbyRestaurants(latitude: number, longitude: number, radiusInKm: number = 5) {
      return api.get(`/api/Restaurant/nearby?latitude=${latitude}&longitude=${longitude}&radiusInKm=${radiusInKm}`)
    } 
  }
}
