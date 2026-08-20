import { defineStore } from 'pinia'

function parseJwt(token: string) {
  try {
    const base64Url = token.split('.')[1]
    if (!base64Url) return null
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    
    let binaryStr: string
    if (typeof atob === 'function') {
      binaryStr = atob(base64)
    } else if (typeof Buffer !== 'undefined') {
      binaryStr = Buffer.from(base64, 'base64').toString('binary')
    } else {
      return null
    }

    const jsonPayload = decodeURIComponent(
      binaryStr
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
    email: decoded.email || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
    restaurantId: decoded.restaurantId ? parseInt(decoded.restaurantId) : null
  }
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    if (process.server) {
      return {
        isLoggedIn: false,
        user: null as any
      }
    }

    const token = useCookie('auth_token', { path: '/' })
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
    async login(email: string, password: string) {
      const authService = useAuthService()

      try {
        const response = await authService.login({ email, password }) as any
        const data = response?.data
        
        if (response?.isSuccess && data?.requiresVerification) {
          return { requiresVerification: true, email: data.email }
        }
        
        const token = data || response?.token || (typeof response === 'string' ? response : null)
        if (token && typeof token === 'string' && token.length > 20) {
          const tokenCookie = useCookie('auth_token', { path: '/', maxAge: 60 * 60 })
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return { requiresVerification: false, success: true }
        }

        return { requiresVerification: false, success: false, error: response?.message || 'Giriş başarısız' }
      } catch (error: any) {
        console.error('Giriş başarısız:', error)
        return { requiresVerification: false, success: false, error: error?.data?.message || 'Giriş işlemi başarısız.' }
      }
    },

    async verifyLogin(email: string, code: string) {
      const authService = useAuthService()
      const tokenCookie = useCookie('auth_token', { path: '/', maxAge: 60 * 60 })

      try {
        const response = await authService.verifyLogin({ email, code }) as any
        const token = response?.data || response?.token

        if (token && typeof token === 'string') {
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return { success: true }
        }
        return { success: false, error: response?.message || 'Doğrulama başarısız.' }
      } catch (error: any) {
        console.error('Doğrulama başarısız:', error)
        return { success: false, error: error?.data?.message || 'Doğrulama işlemi başarısız.' }
      }
    },

    async register(name: string, email: string, password: string, confirmPassword: string) {
      const authService = useAuthService()

      try {
        const response = await authService.register({ name, email, password, confirmPassword }) as any
        const data = response?.data
        
        if (response?.isSuccess && data?.requiresVerification) {
          return { requiresVerification: true, email: data.email }
        }

        const token = data || response?.token || (typeof response === 'string' ? response : null)
        if (token && typeof token === 'string' && token.length > 20) {
          const tokenCookie = useCookie('auth_token', { path: '/', maxAge: 60 * 60 })
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return { requiresVerification: false, success: true }
        }

        return { requiresVerification: false, success: false, error: response?.message || 'Kayıt işlemi başarısız.' }
      } catch (error: any) {
        console.error('Kayıt başarısız:', error)
        return { requiresVerification: false, success: false, error: error?.data?.message || 'Kayıt işlemi başarısız.' }
      }
    },

    async verifyRegister(email: string, code: string) {
      const authService = useAuthService()
      const tokenCookie = useCookie('auth_token', { path: '/', maxAge: 60 * 60 })

      try {
        const response = await authService.verifyRegister({ email, code }) as any
        const token = response?.data || response?.token

        if (token && typeof token === 'string') {
          tokenCookie.value = token
          const decoded = parseJwt(token)
          this.isLoggedIn = true
          this.user = extractUserFromDecoded(decoded)
          useRestaurantStore().resetState()
          return { success: true }
        }
        return { success: false, error: response?.message || 'Doğrulama başarısız.' }
      } catch (error: any) {
        console.error('Doğrulama başarısız:', error)
        return { success: false, error: error?.data?.message || 'Doğrulama işlemi başarısız.' }
      }
    },

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
    },

    updateUserName(newName: string) {
      if (this.user) {
        this.user.name = newName
      }
    }
  }
})