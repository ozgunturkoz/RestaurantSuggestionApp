import { useToast } from '~/composables/useToast'
import { useI18n } from '~/composables/useI18n'

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

      if (status === 401) {
        const authStore = useAuthStore()
        toast.showError(t('common.sessionExpired'), t('common.sessionExpiredTitle'))
        authStore.logout()
        throw new Error(t('common.sessionExpired'))
      }

      if (status === 429) {
        const rateLimitMsg = t('common.rateLimitError')
        toast.showWarning(rateLimitMsg, t('common.rateLimitTitle'))
        const customErr = new Error(rateLimitMsg) as any
        customErr.customMessage = rateLimitMsg
        throw customErr
      }

      const backendMsg = err?.data?.message || err?.data?.errorMessage
      const isServerError = status && status >= 500
      const errorMsg = backendMsg || (isServerError ? t('common.serverError') : t('common.connectionError'))
      const errorTitle = backendMsg ? undefined : (isServerError ? t('common.serverError') : t('common.connectionErrorTitle'))

      toast.showError(errorMsg, errorTitle)
      const customErr = new Error(errorMsg) as any
      customErr.customMessage = errorMsg
      throw customErr
    }
  }

  return {
    get: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'GET', ...options }),
    post: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'POST', body, ...options }),
    put: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'PUT', body, ...options }),
    delete: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'DELETE', ...options }),
  }
}
