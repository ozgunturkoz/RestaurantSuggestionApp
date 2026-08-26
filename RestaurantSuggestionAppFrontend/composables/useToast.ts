import { ref } from 'vue'

// =========================================================================================
// BU COMPOSABLE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Ekranın sağ üst köşesinde beliren renkli bildirim kutucuklarını (Toast Messages) yönetir.
// Örneğin: "Şifreniz değiştirildi", "Giriş başarılı", "E-posta hatalı" gibi uyarıları
// birkaç saniye ekranda gösterir ve ardından yumuşak bir animasyonla kendiliğinden kapatır.
// =========================================================================================

export interface ToastItem {
  id: number
  type: 'error' | 'success' | 'warning' | 'info'
  title?: string
  message: string
  duration?: number
}

// Ekranda o an gösterilen tüm aktif bildirimlerin listesi
const toasts = ref<ToastItem[]>([])
let nextId = 1

export const useToast = () => {
  // Bildirim listesine yeni bir mesaj ekleyen ana fonksiyon
  const addToast = (toast: Omit<ToastItem, 'id'>) => {
    const id = nextId++
    const duration = toast.duration ?? 4500
    const newToast: ToastItem = { ...toast, id }
    
    toasts.value.push(newToast)

    // Belirtilen süre (4.5 saniye) dolunca bildirimi ekrandan otomatik kaldırır
    if (duration > 0) {
      setTimeout(() => {
        removeToast(id)
      }, duration)
    }
  }

  // Bildirimi ID numarasına göre ekrandan silen fonksiyon
  const removeToast = (id: number) => {
    toasts.value = toasts.value.filter(t => t.id !== id)
  }

  // Kırmızı Hata Bildirimi Göster
  const showError = (message: string, title?: string) => {
    addToast({ type: 'error', title, message })
  }

  // Yeşil Başarı Bildirimi Göster
  const showSuccess = (message: string, title?: string) => {
    addToast({ type: 'success', title, message })
  }

  // Sarı Uyarı Bildirimi Göster
  const showWarning = (message: string, title?: string) => {
    addToast({ type: 'warning', title, message })
  }

  // Mavi Bilgi Bildirimi Göster
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
