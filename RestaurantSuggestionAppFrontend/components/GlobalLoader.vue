<template>
  <Transition name="fade">
    <div v-if="show" class="global-loader-overlay">
      <div class="loader-content">
        <div class="logo-pulse">Sofra</div>
        <LoadingSpinner size="lg" />
        <p v-if="text" class="loader-text">{{ text }}</p>
      </div>
    </div>
  </Transition>
</template>

<script setup>
defineProps({
  show: {
    type: Boolean,
    default: false
  },
  text: {
    type: String,
    default: 'Yükleniyor...'
  }
})
</script>

<style scoped>
.global-loader-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(23, 18, 15, 0.85);
  backdrop-filter: blur(8px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.loader-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

.logo-pulse {
  font-family: 'Instrument Serif', serif;
  font-size: 36px;
  font-style: italic;
  color: #E0A458;
  animation: pulse-logo 1.5s ease-in-out infinite;
}

.loader-text {
  font-family: 'Manrope', sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #C9B9A8;
  margin: 0;
}

@keyframes pulse-logo {
  0%, 100% {
    opacity: 0.6;
    transform: scale(0.98);
  }
  50% {
    opacity: 1;
    transform: scale(1.04);
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
