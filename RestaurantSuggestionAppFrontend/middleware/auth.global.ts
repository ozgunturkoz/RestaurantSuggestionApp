import { useAuthStore } from '~/stores/authStore.ts'

export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // 1. Giriş yapılmadıysa login sayfasına yönlendir (login ve register sayfaları hariç)
  if (!authStore.isLoggedIn && to.path !== '/login' && to.path !== '/register') {
    return navigateTo('/login')
  }

  // 2. Giriş yapılmışsa ve login/register sayfalarına gitmeye çalışıyorsa
  if (authStore.isLoggedIn && (to.path === '/login' || to.path === '/register')) {
    if (authStore.isRestaurantOwner) {
      return navigateTo('/owner-dashboard')
    }
    return navigateTo('/')
  }

  // 3. Giriş yapan Restoran Sahibi ise ve müşteri sayfalarına (Harita) girmeye çalışıyorsa
  if (authStore.isLoggedIn && authStore.isRestaurantOwner) {
    if (to.path === '/') {
      return navigateTo('/owner-dashboard')
    }
  }

  // 4. Giriş yapan Müşteri ise ve Restoran Sahibi paneline girmeye çalışıyorsa
  if (authStore.isLoggedIn && !authStore.isRestaurantOwner && to.path === '/owner-dashboard') {
    return navigateTo('/')
  }
})