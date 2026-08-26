// =========================================================================================
// NUXT YAPILANDIRMA DOSYASI (NUXT.CONFIG.TS) (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Bu dosya, web sitemizin (Frontend) tüm çalışma ayarlarını belirler.
// Sunucu portu, HTTPS sertifikası, arka plan (API) bağlantı adresleri, yazı tipleri (fontlar)
// ve eklenen eklentiler (Pinia, Leaflet vb.) buradan kontrol edilir.
// =========================================================================================

export default defineNuxtConfig({
  // Tarayıcı tabanlı Tek Sayfalı Uygulama (SPA) modu
  ssr: false,

  // Geliştirme sunucusu ayarları (Yerel ağdan ve mobil cihazlardan erişim için)
  devServer: {
    host: '0.0.0.0',
    port: 3000,
    https: true
  },

  // Vite ve Nuxt Rollup yapılandırma kancası:
  // SPA modunda (ssr: false) Vite 7 builder'ın giriş noktası hatasını (rollupOptions.input) engeller.
  hooks: {
    'vite:extendConfig'(config) {
      if (config.build?.rollupOptions?.input && typeof config.build.rollupOptions.input === 'object' && !Array.isArray(config.build.rollupOptions.input)) {
        const input = config.build.rollupOptions.input as Record<string, any>;
        if (input.entry && !input.server) {
          input.server = input.entry;
        }
      }
    }
  },

  // Backend API yönlendirme köprüsü (Proxy)
  nitro: {
    devProxy: {
      '/api': {
        target: 'https://localhost:7085/api',
        secure: false,
        changeOrigin: true
      }
    }
  },

  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },

  // Durum yönetim kütüphanesi (Pinia) modülü
  modules: [
    '@pinia/nuxt'
  ],

  imports: {
    dirs: ['stores']
  },

  pinia: {
    storesDirs: ['./stores/**']
  },

  // Sayfada kullanılan global stil dosyaları (Harita CSS ve Giriş CSS)
  css: [
    'leaflet/dist/leaflet.css',
    '~/assets/auth.css'
  ],
  
  // HTML Başlık ve Google Fonts bağlantıları + Beyaz Çerçeve Sıfırlaması
  app: {
    head: {
      meta: [
        { name: 'viewport', content: 'width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' }
      ],
      link: [
        { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&family=Instrument+Serif:ital@0;1&display=swap' }
      ],
      style: [
        { children: 'html,body,#__nuxt{margin:0!important;padding:0!important;border:none!important;outline:none!important;background:#17120F!important;background-color:#17120F!important;width:100%!important;min-height:100vh!important;overflow-x:hidden!important;}' }
      ]
    }
  },

  // =========================================================================================
  // OTOMATİK LOCAL / CANLI API GEÇİŞİ:
  // 1. Bilgisayarınızda test ederken (npm run dev): Otomatik olarak "https://localhost:7085" adresine bağlanır.
  // 2. Canlıya atıldığında (Production): Otomatik olarak "https://restaurantsuggestionapp.onrender.com" adresine bağlanır.
  // 3. İstenirse .env dosyasındaki NUXT_PUBLIC_API_BASE değeri ile manuel olarak da ezilebilir.
  // =========================================================================================
  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || (process.env.NODE_ENV === 'development' ? 'https://localhost:7085' : 'https://restaurantsuggestionapp.onrender.com')
    }
  }
})