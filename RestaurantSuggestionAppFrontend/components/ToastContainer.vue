<template>
  <ClientOnly>
    <Teleport to="body">
      <div class="toast-container" role="region" aria-live="polite">
        <TransitionGroup name="toast-anim" tag="div" class="toast-list">
          <div
            v-for="toast in toasts"
            :key="toast.id"
            :class="['toast-item', `toast-${toast.type}`]"
          >
            <div class="toast-icon-wrap">
              <span v-if="toast.type === 'error'" class="toast-icon icon-error">✕</span>
              <span v-else-if="toast.type === 'success'" class="toast-icon icon-success">✓</span>
              <span v-else-if="toast.type === 'warning'" class="toast-icon icon-warning">!</span>
              <span v-else class="toast-icon icon-info">i</span>
            </div>

            <div class="toast-body">
              <h4 v-if="toast.title" class="toast-title">{{ toast.title }}</h4>
              <p class="toast-msg">{{ toast.message }}</p>
            </div>

            <button
              class="toast-close"
              type="button"
              :aria-label="t('common.close')"
              @click="removeToast(toast.id)"
            >
              ✕
            </button>
          </div>
        </TransitionGroup>
      </div>
    </Teleport>
  </ClientOnly>
</template>

<script setup lang="ts">
import { useToast } from '~/composables/useToast'
import { useI18n } from '~/composables/useI18n'

const { toasts, removeToast } = useToast()
const { t } = useI18n()
</script>

<style>
.toast-container {
  position: fixed;
  top: 24px;
  right: 24px;
  z-index: 999999;
  max-width: 380px;
  width: calc(100vw - 48px);
  pointer-events: none;
  display: flex;
  flex-direction: column;
}

.toast-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
  width: 100%;
}

.toast-item {
  pointer-events: auto;
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 14px 16px;
  border-radius: 12px;
  background: #201A16;
  border: 1px solid #3A2F25;
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.55), 0 2px 6px rgba(0, 0, 0, 0.3);
  color: #F3E9DE;
  font-family: 'Manrope', -apple-system, BlinkMacSystemFont, sans-serif;
  box-sizing: border-box;
}

.toast-item.toast-error {
  border-left: 4px solid #ef4444;
  background: linear-gradient(135deg, rgba(239, 68, 68, 0.12) 0%, #201A16 100%);
}

.toast-item.toast-success {
  border-left: 4px solid #10b981;
  background: linear-gradient(135deg, rgba(16, 185, 129, 0.12) 0%, #201A16 100%);
}

.toast-item.toast-warning {
  border-left: 4px solid #E0A458;
  background: linear-gradient(135deg, rgba(224, 164, 88, 0.12) 0%, #201A16 100%);
}

.toast-item.toast-info {
  border-left: 4px solid #3b82f6;
  background: linear-gradient(135deg, rgba(59, 130, 246, 0.12) 0%, #201A16 100%);
}

.toast-icon-wrap {
  flex-shrink: 0;
  margin-top: 1px;
}

.toast-icon {
  width: 22px;
  height: 22px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 11px;
  font-weight: 800;
  line-height: 1;
}

.icon-error {
  background: rgba(239, 68, 68, 0.2);
  color: #ef4444;
  border: 1px solid rgba(239, 68, 68, 0.4);
}

.icon-success {
  background: rgba(16, 185, 129, 0.2);
  color: #10b981;
  border: 1px solid rgba(16, 185, 129, 0.4);
}

.icon-warning {
  background: rgba(224, 164, 88, 0.2);
  color: #E0A458;
  border: 1px solid rgba(224, 164, 88, 0.4);
}

.icon-info {
  background: rgba(59, 130, 246, 0.2);
  color: #3b82f6;
  border: 1px solid rgba(59, 130, 246, 0.4);
}

.toast-body {
  flex: 1;
  min-width: 0;
}

.toast-title {
  margin: 0 0 3px 0 !important;
  font-size: 13.5px !important;
  font-weight: 700 !important;
  color: #F3E9DE !important;
  line-height: 1.3 !important;
}

.toast-msg {
  margin: 0 !important;
  font-size: 12.5px !important;
  color: #C9B9A8 !important;
  line-height: 1.45 !important;
  word-break: break-word !important;
}

.toast-close {
  flex-shrink: 0;
  background: transparent !important;
  border: none !important;
  outline: none !important;
  color: #8A745E !important;
  cursor: pointer;
  padding: 2px 5px !important;
  font-size: 13px !important;
  line-height: 1;
  border-radius: 4px;
  transition: color 0.15s, background 0.15s;
}

.toast-close:hover {
  color: #F3E9DE !important;
  background: rgba(255, 255, 255, 0.08) !important;
}

/* Animations */
.toast-anim-enter-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-anim-leave-active {
  transition: all 0.25s cubic-bezier(0.4, 0, 1, 1);
}

.toast-anim-enter-from {
  opacity: 0;
  transform: translateX(40px) scale(0.95);
}

.toast-anim-leave-to {
  opacity: 0;
  transform: translateX(40px) scale(0.95);
}

@media (max-width: 600px) {
  .toast-container {
    top: 14px;
    right: 14px;
    left: 14px;
    width: calc(100vw - 28px);
    max-width: none;
  }
}
</style>
