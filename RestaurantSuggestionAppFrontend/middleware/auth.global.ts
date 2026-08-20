import { useAuthStore } from '~/stores/authStore.ts'

export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // 1. Giriş yapılmadıysa login sayfasına yönlendir (login ve register sayfaları hariç)
  if (!authStore.isLoggedIn && to.path !== '/login' && to.path !== '/register') {
    return navigateTo('/login')
  }

  // 2. Giriş yapılmışsa ve login/register sayfalarına gitmeye çalışıyorsa ana sayfaya yönlendir
  if (authStore.isLoggedIn && (to.path === '/login' || to.path === '/register')) {
    return navigateTo('/')
  }
})