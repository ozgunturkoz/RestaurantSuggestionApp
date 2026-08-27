export const useAuthService = () => {
  const api = useApi()

  return {
    login(credentials: { email: string; password: string }) {
      return api.post('/api/Auth/login', credentials)
    },

    verifyLogin(data: { email: string; code: string }) {
      return api.post('/api/Auth/verify-login', data)
    },

    register(userData: { name: string; email: string; password: string; confirmPassword: string }) {
      return api.post('/api/Auth/register', userData)
    },

    verifyRegister(data: { email: string; code: string }) {
      return api.post('/api/Auth/verify-register', data)
    },

    forgotPassword(dataOrEmail: string | { email: string }) {
      const body = typeof dataOrEmail === 'string' ? { email: dataOrEmail } : dataOrEmail
      return api.post('/api/Auth/forgot-password', body)
    },

    resetPassword(data: { email: string; code: string; newPassword: string; confirmPassword: string }) {
      return api.post('/api/Auth/reset-password', data)
    }
  }
}
