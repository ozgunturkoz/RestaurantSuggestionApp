<template>
  <!-- =========================================================================================
       ANA UYGULAMA KÖK ŞABLONU (APP.VUE) (KODLAMA BİLMEYENLER İÇİN ANLATIM)
       Bu dosya, tüm web sitesinin ana çatısıdır.
       Hangi sayfaya (Giriş, Kayıt, Keşfet, Profil) gidilirse gidilsin,
       en tepedeki altın sarısı yüklenme çizgisi, aktif sayfa ve bildirim kutucukları (Toast)
       bu çatı içinde görüntülenir.
       ========================================================================================= -->
  <div class="app-root">
    <!-- Sayfa geçişlerinde ekranın en üstünde beliren altın sarısı ilerleme çubuğu -->
    <NuxtLoadingIndicator color="#E0A458" :height="3" />

    <!-- O an görüntülenen sayfanın (pages/index.vue, pages/profile.vue vb.) yerleştiği alan -->
    <NuxtPage />

    <!-- Ekranın sağ üst köşesindeki renkli bildirim kutularının kapsayıcısı -->
    <ToastContainer />
  </div>
</template>

<script setup>
// =========================================================================================
// KÖK BAŞLATMA KODLARI (JAVASCRIPT)
// =========================================================================================
import { onMounted } from 'vue'
import { useI18n } from '~/composables/useI18n'

// Çoklu dil yöneticisini çağırıyoruz
const { initLocale } = useI18n()

// Web sitesi ilk yüklendiğinde kullanıcının son seçtiği dili (TR veya EN) otomatik yükler
onMounted(() => {
  initLocale()
})
</script>

<style>
/* =========================================================================================
   GENEL WEB SİTESİ STİLLERİ (GLOBAL CSS RESET)
   Tüm sayfalarda beyaz kenarlığı, varsayılan tarayıcı boşluklarını ve sınırları sıfırlar.
   ========================================================================================= */
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

html, body, #__nuxt, #__layout, .app-root {
  margin: 0 !important;
  padding: 0 !important;
  border: none !important;
  outline: none !important;
  box-shadow: none !important;
  background: #17120F !important;
  background-color: #17120F !important;
  color: #F3E9DE;
  font-family: 'Manrope', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  width: 100% !important;
  min-height: 100vh !important;
  overflow-x: hidden !important;
}
</style>