import { useToast } from '~/composables/useToast'
import { useI18n } from '~/composables/useI18n'

// =========================================================================================
// BU COMPOSABLE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Frontend'in backend ile konuşurken kullandığı "Genel Posta / Kargo Görevlisidir".
// Her istek atıldığında kullanıcının dil tercihini (TR / EN) ve giriş anahtarını (JWT Token)
// başlığa otomatik ekler. Sunucu bir hata verirse (örneğin oturum süresi dolduysa)
// kullanıcıyı uyarır ve güvenle giriş sayfasına yönlendirir.
// =========================================================================================

export const useApi = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase
  const toast = useToast()

  const customFetch = async (endpoint: string, options: any = {}) => {
    const token = useCookie('auth_token', { path: '/' })
    const { locale, t } = useI18n()

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
        const errorMsg = response.message || response.errorMessage || t('common.error')
        toast.showError(errorMsg)
      }

      return response
    } catch (err: any) {
      const status = err?.response?.status || err?.status || err?.statusCode || err?.data?.statusCode
      
      // 401 Unauthorized: Oturum süresi dolmuşsa
      if (status === 401) {
        const authStore = useAuthStore()
        toast.showError(t('common.sessionExpired'), t('common.sessionExpiredTitle'))
        authStore.logout()
        throw err
      }

      const errorMsg = err?.data?.message || err?.data?.errorMessage || err?.message || t('common.connectionError')
      toast.showError(errorMsg, t('common.connectionErrorTitle'))
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