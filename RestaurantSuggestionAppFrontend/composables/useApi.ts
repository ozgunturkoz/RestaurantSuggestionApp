import { useToast } from '~/composables/useToast'

export const useApi = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase
  const toast = useToast()

  const customFetch = async (endpoint: string, options: any = {}) => {
    const token = useCookie('auth_token', { path: '/' })
    const { locale } = useI18n()

    try {
      const response: any = await $fetch(`${apiBase}${endpoint}`, {
        ...options,
        headers: {
          'Content-Type': 'application/json',
          'Accept-Language': locale.value || 'tr',
          ...(token.value ? { 'Authorization': `Bearer ${token.value}` } : {}),
          ...options.headers,
        }
      })

      if (response && response.isSuccess === false) {
        const errorMsg = response.message || response.errorMessage || 'Bir hata oluştu.'
        toast.showError(errorMsg)
      }

      return response
    } catch (err: any) {
      const status = err?.response?.status || err?.status || err?.statusCode || err?.data?.statusCode
      
      if (status === 401) {
        const authStore = useAuthStore()
        toast.showError('Oturumunuzun süresi doldu. Lütfen tekrar giriş yapın.', 'Oturum Süresi Doldu')
        authStore.logout()
        throw err
      }

      const errorMsg = err?.data?.message || err?.data?.errorMessage || err?.message || 'Sunucu ile iletişim kurulurken bir hata oluştu.'
      toast.showError(errorMsg, 'Bağlantı/Sunucu Hatası')
      throw err
    }
  }

  return {
    get: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'GET', ...options }),
    post: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'POST', body, ...options }),
    put: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'PUT', body, ...options }),
    delete: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'DELETE', ...options }),
  }
}