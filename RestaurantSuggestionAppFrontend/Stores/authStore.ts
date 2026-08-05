import { defineStore } from 'pinia'

// JWT token'ın payload kısmını çözümlemek için yardımcı fonksiyon
function parseJwt(token: string) {
  try {
    const base64Url = token.split('.')[1]
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    )
    return JSON.parse(jsonPayload)
  } catch (e) {
    return null;
  }
}

function extractUserFromDecoded(decoded: any) {
  if (!decoded) return null
  return {
    id: decoded.nameid || decoded.sub || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
    name: decoded.unique_name || decoded.name || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
    email: decoded.email || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress']
  }
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    const token = useCookie('auth_token')
    const rawToken = token.value

    if (rawToken && rawToken !== 'null' && rawToken !== 'undefined' && rawToken.length > 10) {
      const decoded = parseJwt(rawToken)
      const user = extractUserFromDecoded(decoded)
      if (user) {
        return {
          isLoggedIn: true,
          user
        }
      }
    }

    return {
      isLoggedIn: false,
      user: null as any
    }
  },
  actions: {
    // GİRİŞ İŞLEMİ
    async login(email: string, password: string) {
      const authService = useAuthService()
      const tokenCookie = useCookie('auth_token', { maxAge: 60 * 60 })

      try {
        const response = await authService.login({ email, password }) as any
        const token = response?.data || response?.token || (typeof response === 'string' ? response : null)
        
        if (token) {
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return true
        }
        return false
      } catch (error) {
        console.error('Giriş başarısız:', error)
        return false
      }
    },

    // KAYIT İŞLEMİ
    async register(name: string, email: string, password: string) {
      const authService = useAuthService()
      const tokenCookie = useCookie('auth_token', { maxAge: 60 * 60 })

      try {
        const response = await authService.register({ name, email, password }) as any
        const token = response?.data || response?.token || (typeof response === 'string' ? response : null)
        
        if (token) {
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return true
        }
        return false
      } catch (error) {
        console.error('Kayıt başarısız:', error)
        return false
      }
    },

    // ÇIKIŞ İŞLEMİ
    logout() {
      const tokenCookie = useCookie('auth_token', { path: '/' })
      tokenCookie.value = undefined
      this.isLoggedIn = false
      this.user = null
      useRestaurantStore().resetState()
      
      if (typeof window !== 'undefined') {
        window.location.href = '/login'
      } else {
        return navigateTo('/login', { replace: true })
      }
    }
  }
})