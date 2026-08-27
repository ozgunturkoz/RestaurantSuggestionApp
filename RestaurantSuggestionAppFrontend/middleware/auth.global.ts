export default defineNuxtRouteMiddleware((to, from) => {
  if (process.server) return

  const authStore = useAuthStore()

  if (!authStore.isLoggedIn && to.path !== '/login' && to.path !== '/register') {
    return navigateTo('/login')
  }

  if (authStore.isLoggedIn && (to.path === '/login' || to.path === '/register')) {
    return navigateTo('/')
  }
})
