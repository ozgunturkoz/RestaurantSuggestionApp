import { useAuthStore } from '~/stores/authStore'

export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // Giriş yapmamış kullanıcıların hem /login hem de /register sayfalarına girmesine izin veriyoruz:
  if (!authStore.isLoggedIn && to.path !== '/login' && to.path !== '/register') {
    return navigateTo('/login')
  }

  // Giriş yapmış bir kullanıcı tekrar login veya register sayfalarına girmeye çalışırsa ana sayfaya yönlendiriyoruz:
  if (authStore.isLoggedIn && (to.path === '/login' || to.path === '/register')) {
    return navigateTo('/')
  }
})