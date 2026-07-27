// composables/useApi.ts
export const useApi = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase

  const customFetch = async (endpoint: string, options: any = {}) => {
    // Tarayıcıdaki 'auth_token' isimli çerezi okuyoruz
    const token = useCookie('auth_token')

    return await $fetch(`${apiBase}${endpoint}`, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        // Eğer token varsa header'a Authorization Bearer olarak ekliyoruz
        ...(token.value ? { 'Authorization': `Bearer ${token.value}` } : {}),
        ...options.headers,
      }
    })
  }

  return {
    get: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'GET', ...options }),
    post: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'POST', body, ...options }),
    put: (endpoint: string, body: any, options = {}) => customFetch(endpoint, { method: 'PUT', body, ...options }),
    delete: (endpoint: string, options = {}) => customFetch(endpoint, { method: 'DELETE', ...options }),
  }
}