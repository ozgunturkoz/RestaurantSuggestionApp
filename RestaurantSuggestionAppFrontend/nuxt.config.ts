export default defineNuxtConfig({
  ssr: false,

  devServer: {
    host: '0.0.0.0',
    port: 3000,
    https: true
  },

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
      meta: [
        { name: 'viewport', content: 'width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' }
      ],
      link: [
        { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&family=Instrument+Serif:ital@0;1&display=swap' },
        { rel: 'stylesheet', href: 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' }
      ],
      style: [
        { children: 'html,body,#__nuxt{margin:0!important;padding:0!important;border:none!important;outline:none!important;background:#17120F!important;background-color:#17120F!important;width:100%!important;min-height:100vh!important;overflow-x:hidden!important;}' }
      ]
    }
  },

  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || (process.env.NODE_ENV === 'development' ? 'https://localhost:7085' : 'https://restaurantsuggestionapp.onrender.com')
    }
  }
})
