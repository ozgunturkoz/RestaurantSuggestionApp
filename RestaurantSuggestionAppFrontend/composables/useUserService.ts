export const useUserService = () => {
  const api = useApi()

  return {
    getExpenditures() {
      return api.get('/api/User/expenditures')
    },
    addExpenditure(dto: { restaurantId?: number | null; amount: number; spentAt?: string }) {
      return api.post('/api/User/expenditures', dto)
    },
    deleteExpenditure(id: number) {
      return api.delete(`/api/User/expenditures/${id}`)
    },
    updateProfile(dto: { name: string }) {
      return api.put('/api/User/profile', dto)
    },
    sendPasswordCode() {
      return api.post('/api/User/send-password-code')
    },
    changePasswordWithCode(dto: { code: string; newPassword: string }) {
      return api.post('/api/User/change-password-with-code', dto)
    },
    sendEmailChangeCode(dto: { newEmail: string }) {
      return api.post('/api/User/send-email-change-code', dto)
    },
    changeEmailWithCode(dto: { newEmail: string; code: string; currentPassword: string }) {
      return api.post('/api/User/change-email-with-code', dto)
    },
    getFavorites() {
      return api.get('/api/User/favorites')
    },
    removeFavorite(restaurantId: number) {
      return api.delete(`/api/User/favorites/${restaurantId}`)
    }
  }
}
