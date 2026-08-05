export const useAuthService = () => {
  const api = useApi()

  return {
    login(credentials: { email: string; password: string }) {
      return api.post('/api/Auth/login', credentials)
    },
    verifyLogin(data: { email: string; code: string }) {
      return api.post('/api/Auth/verify-login', data)
    },
    register(userData: { name: string; email: string; password: string }) {
      return api.post('/api/Auth/register', userData)
    }
  }
}