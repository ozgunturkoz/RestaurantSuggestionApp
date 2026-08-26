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

/* =========================================================================================
   LEAFLET HARİTA KARELERİ (TILES) DÜZELTMESİ (KODLAMA BİLMEYENLER İÇİN ANLATIM)
   Harita parçacıklarının (karelerin) birbiri üzerine binmesini veya yamulmasını engeller.
   ========================================================================================= */
.leaflet-container {
  height: 100% !important;
  width: 100% !important;
  background: #17120F !important;
  font-family: 'Manrope', sans-serif !important;
  outline: none !important;
}

.leaflet-pane,
.leaflet-tile-pane,
.leaflet-map-pane,
.leaflet-layer,
.leaflet-tile-container {
  position: absolute !important;
  left: 0 !important;
  top: 0 !important;
}

.leaflet-tile {
  position: absolute !important;
  left: 0 !important;
  top: 0 !important;
  max-width: none !important;
  max-height: none !important;
  width: 256px !important;
  height: 256px !important;
  user-select: none !important;
  -webkit-user-select: none !important;
  -webkit-user-drag: none !important;
}

.leaflet-tile-container img {
  max-width: none !important;
  max-height: none !important;
  width: 256px !important;
  height: 256px !important;
}
</style>