import { ref } from 'vue'

export interface ToastItem {
  id: number
  type: 'error' | 'success' | 'warning' | 'info'
  title?: string
  message: string
  duration?: number
}

const toasts = ref<ToastItem[]>([])
let nextId = 1

export const useToast = () => {
  const addToast = (toast: Omit<ToastItem, 'id'>) => {
    const id = nextId++
    const duration = toast.duration ?? 4500
    const newToast: ToastItem = { ...toast, id }

    toasts.value.push(newToast)

    if (duration > 0) {
      setTimeout(() => {
        removeToast(id)
      }, duration)
    }
  }

  const removeToast = (id: number) => {
    toasts.value = toasts.value.filter(t => t.id !== id)
  }

  const showError = (message: string, title?: string) => {
    addToast({ type: 'error', title, message })
  }

  const showSuccess = (message: string, title?: string) => {
    addToast({ type: 'success', title, message })
  }

  const showWarning = (message: string, title?: string) => {
    addToast({ type: 'warning', title, message })
  }

  const showInfo = (message: string, title?: string) => {
    addToast({ type: 'info', title, message })
  }

  return {
    toasts,
    addToast,
    removeToast,
    showError,
    showSuccess,
    showWarning,
    showInfo
  }
}
