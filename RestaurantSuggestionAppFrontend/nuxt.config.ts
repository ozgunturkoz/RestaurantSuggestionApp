export default defineNuxtConfig({
  ssr: false,

  devServer: {
    host: '0.0.0.0',
    port: 3000,
    https: true
  },

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
  modules: [
    '@pinia/nuxt'
  ],
  imports: {
    dirs: ['stores']
  },
  pinia: {
    storesDirs: ['./stores/**']
  },

  css: [
    'leaflet/dist/leaflet.css',
    '~/assets/auth.css'
  ],
  
  app: {
    head: {
      link: [
        { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&family=Instrument+Serif:ital@0;1&display=swap' }
      ]
    }
  },

  runtimeConfig: {
    public: {
      apiBase: ''
    }
  }
})