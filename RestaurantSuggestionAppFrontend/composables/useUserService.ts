// =========================================================================================
// BU COMPOSABLE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Profil sayfasında kullanıcının yaptığı tüm işlemleri (harcamaları çekme/ekleme/silme,
// profil adını güncelleme, e-posta ve şifre değiştirme, favorileri yönetme) backend sunucusuna iletir.
// =========================================================================================

export const useUserService = () => {
  const api = useApi()

  return {
    // Kullanıcının geçmiş harcama listesini getirir
    getExpenditures() {
      return api.get('/api/User/expenditures')
    },

    // Yeni bir harcama kaydı ekler
    addExpenditure(dto: { restaurantId?: number | null; amount: number; spentAt?: string }) {
      return api.post('/api/User/expenditures', dto)
    },

    // Harcama kaydını siler
    deleteExpenditure(id: number) {
      return api.delete(`/api/User/expenditures/${id}`)
    },

    // Profil adını günceller
    updateProfile(dto: { name: string }) {
      return api.put('/api/User/profile', dto)
    },

    // Profil içinden şifre değiştirmek için onay kodu ister
    sendPasswordCode() {
      return api.post('/api/User/send-password-code')
    },

    // Onay kodu ve yeni şifre ile şifreyi değiştirir
    changePasswordWithCode(dto: { code: string; newPassword: string }) {
      return api.post('/api/User/change-password-with-code', dto)
    },

    // Yeni e-postaya doğrulama kodu ister
    sendEmailChangeCode(dto: { newEmail: string }) {
      return api.post('/api/User/send-email-change-code', dto)
    },

    // Doğrulama kodu ve mevcut şifre ile e-postayı değiştirir
    changeEmailWithCode(dto: { newEmail: string; code: string; currentPassword: string }) {
      return api.post('/api/User/change-email-with-code', dto)
    },

    // Favori restoranları listeler
    getFavorites() {
      return api.get('/api/User/favorites')
    },

    // Restoranı favorilerden çıkarır
    removeFavorite(restaurantId: number) {
      return api.delete(`/api/User/favorites/${restaurantId}`)
    }
  }
}
