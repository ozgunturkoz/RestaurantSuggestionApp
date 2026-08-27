<template>

  <div class="spinner-container" :class="[sizeClass, { 'inline': inline }]">
    <svg class="spinner-svg" viewBox="0 0 50 50">
      <circle
        class="spinner-track"
        cx="25"
        cy="25"
        r="20"
        fill="none"
        stroke-width="5"
      ></circle>
      <circle
        class="spinner-head"
        cx="25"
        cy="25"
        r="20"
        fill="none"
        stroke-width="5"
      ></circle>
    </svg>
    <span v-if="text" class="spinner-text">{{ text }}</span>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  size: {
    type: String,
    default: 'md'
  },
  inline: {
    type: Boolean,
    default: false
  },
  text: {
    type: String,
    default: ''
  }
})

const sizeClass = computed(() => `size-${props.size}`)

</script>

<style scoped>
.spinner-container {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.spinner-container.inline {
  display: inline-flex;
}

.spinner-svg {
  animation: rotate 1.4s linear infinite;
  transform-origin: center center;
}

.size-sm .spinner-svg {
  width: 16px;
  height: 16px;
}

.size-md .spinner-svg {
  width: 24px;
  height: 24px;
}

.size-lg .spinner-svg {
  width: 40px;
  height: 40px;
}

.spinner-track {
  stroke: rgba(224, 164, 88, 0.2);
}

.spinner-head {
  stroke: #E0A458;
  stroke-dasharray: 90, 150;
  stroke-dashoffset: 0;
  stroke-linecap: round;
  animation: dash 1.4s ease-in-out infinite;
}

.spinner-text {
  font-size: 13px;
  font-weight: 600;
  color: #E0A458;
}

@keyframes rotate {
  100% {
    transform: rotate(360deg);
  }
}

@keyframes dash {
  0% {
    stroke-dasharray: 1, 200;
    stroke-dashoffset: 0;
  }
  50% {
    stroke-dasharray: 89, 200;
    stroke-dashoffset: -35px;
  }
  100% {
    stroke-dasharray: 89, 200;
    stroke-dashoffset: -124px;
  }
}
</style>
