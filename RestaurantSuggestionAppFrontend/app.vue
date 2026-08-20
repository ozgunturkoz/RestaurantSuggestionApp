<template>
  <div>
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