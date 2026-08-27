<template>

  <div class="toast-container">
    <TransitionGroup name="toast-fade">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="['toast-card', `toast-${toast.type}`]"
      >
        <div class="toast-icon">
          <span v-if="toast.type === 'error'">❌</span>
          <span v-else-if="toast.type === 'success'">✅</span>
          <span v-else-if="toast.type === 'warning'">⚠️</span>
          <span v-else>ℹ️</span>
        </div>

        <div class="toast-content">
          <h4 v-if="toast.title" class="toast-title">{{ toast.title }}</h4>
          <p class="toast-message">{{ toast.message }}</p>
        </div>

        <button class="toast-close-btn" @click="removeToast(toast.id)" :aria-label="t('common.close')">
          ✕
        </button>
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup lang="ts">
import { useToast } from '~/composables/useToast'
import { useI18n } from '~/composables/useI18n'

const { toasts, removeToast } = useToast()
const { t } = useI18n()

</script>

<style scoped>
.toast-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 99999;
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-width: 380px;
  width: calc(100vw - 40px);
  pointer-events: none;
}

.toast-card {
  pointer-events: auto;
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 14px 16px;
  border-radius: 12px;
  background: rgba(18, 24, 38, 0.92);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border: 1px solid rgba(255, 255, 255, 0.12);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.35);
  color: #f3f4f6;
  font-family: inherit;
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-error {
  border-left: 4px solid #ef4444;
  background: linear-gradient(135deg, rgba(239, 68, 68, 0.15) 0%, rgba(18, 24, 38, 0.95) 100%);
}

.toast-success {
  border-left: 4px solid #10b981;
  background: linear-gradient(135deg, rgba(16, 185, 129, 0.15) 0%, rgba(18, 24, 38, 0.95) 100%);
}

.toast-warning {
  border-left: 4px solid #f59e0b;
  background: linear-gradient(135deg, rgba(245, 158, 11, 0.15) 0%, rgba(18, 24, 38, 0.95) 100%);
}

.toast-info {
  border-left: 4px solid #3b82f6;
  background: linear-gradient(135deg, rgba(59, 130, 246, 0.15) 0%, rgba(18, 24, 38, 0.95) 100%);
}

.toast-icon {
  font-size: 1.25rem;
  line-height: 1;
  margin-top: 2px;
}

.toast-content {
  flex: 1;
}

.toast-title {
  margin: 0 0 2px 0;
  font-size: 0.95rem;
  font-weight: 600;
  color: #ffffff;
}

.toast-message {
  margin: 0;
  font-size: 0.875rem;
  color: #d1d5db;
  line-height: 1.4;
  word-break: break-word;
}

.toast-close-btn {
  background: transparent;
  border: none;
  color: #9ca3af;
  cursor: pointer;
  padding: 2px 6px;
  font-size: 14px;
  border-radius: 4px;
  transition: all 0.2s ease;
}

.toast-close-btn:hover {
  color: #ffffff;
  background: rgba(255, 255, 255, 0.1);
}

.toast-fade-enter-active,
.toast-fade-leave-active {
  transition: all 0.3s ease;
}

.toast-fade-enter-from {
  opacity: 0;
  transform: translateX(30px) scale(0.9);
}

.toast-fade-leave-to {
  opacity: 0;
  transform: translateX(30px) scale(0.9);
}
</style>
