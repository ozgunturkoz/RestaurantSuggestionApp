<template>
  <!-- =========================================================================================
       ANA UYGULAMA KÖK ŞABLONU (APP.VUE) (KODLAMA BİLMEYENLER İÇİN ANLATIM)
       Bu dosya, tüm web sitesinin ana çatısıdır.
       Hangi sayfaya (Giriş, Kayıt, Keşfet, Profil) gidilirse gidilsin,
       en tepedeki altın sarısı yüklenme çizgisi, aktif sayfa ve bildirim kutucukları (Toast)
       bu çatı içinde görüntülenir.
       ========================================================================================= -->
  <div>
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