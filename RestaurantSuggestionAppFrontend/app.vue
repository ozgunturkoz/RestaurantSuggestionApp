<template>

  <div class="app-root">

    <NuxtLoadingIndicator color="#E0A458" :height="3" />

    <NuxtPage />

    <ToastContainer />
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useI18n } from '~/composables/useI18n'

const { initLocale } = useI18n()

onMounted(() => {
  initLocale()
})

</script>

<style>
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

.map-tiles-dark {
  filter: brightness(0.6) invert(1) contrast(3) hue-rotate(200deg) saturate(0.3) brightness(0.7) !important;
}

.leaflet-control-attribution {
  background: rgba(23, 18, 15, 0.8) !important;
  color: #8c827a !important;
  font-size: 10px !important;
  padding: 2px 6px !important;
  border-radius: 4px 0 0 0 !important;
}

.leaflet-control-attribution a {
  color: #E0A458 !important;
  text-decoration: none !important;
}

.leaflet-bar a {
  background-color: #241D17 !important;
  color: #F3E9DE !important;
  border-color: #3A2F25 !important;
}

.leaflet-bar a:hover {
  background-color: #2F261E !important;
  color: #E0A458 !important;
}
</style>
