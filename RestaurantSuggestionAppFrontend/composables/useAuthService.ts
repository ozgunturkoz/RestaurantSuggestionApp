// composables/useAuthService.ts
export const useAuthService = () => {
  const api = useApi()

  return {
    login(credentials: { email: string; password: string }) {
      return api.post('/api/Auth/login', credentials)
    },
    // Kayıt olma metodu eklendi
    register(userData: { name: string; email: string; password: string }) {
      return api.post('/api/Auth/register', userData)
    }
  }
}