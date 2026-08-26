// =========================================================================================
// BU COMPOSABLE NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Kullanıcının kimlik doğrulama (Giriş yapma, Kayıt olma, Şifremi unuttum, E-posta doğrulama)
// işlemlerinde frontend tarafından backend API sunucusuna istek gönderen aracı köprüdür.
// =========================================================================================

export const useAuthService = () => {
  // Arka plana güvenli HTTP istekleri atan genel API istemcimiz
  const api = useApi()

  return {
    // Giriş yapma isteği gönderir (E-posta ve Şifre)
    login(credentials: { email: string; password: string }) {
      return api.post('/api/Auth/login', credentials)
    },

    // Giriş doğrulama kodunu gönderir (E-posta ve 6 haneli kod)
    verifyLogin(data: { email: string; code: string }) {
      return api.post('/api/Auth/verify-login', data)
    },

    // Yeni üye kayıt isteği gönderir (Ad, E-posta, Şifre, Şifre Tekrarı)
    register(userData: { name: string; email: string; password: string; confirmPassword: string }) {
      return api.post('/api/Auth/register', userData)
    },

    // Kayıt doğrulama kodunu gönderir
    verifyRegister(data: { email: string; code: string }) {
      return api.post('/api/Auth/verify-register', data)
    },

    // Şifremi unuttum kod isteme isteği gönderir (E-posta)
    forgotPassword(data: { email: string }) {
      return api.post('/api/Auth/forgot-password', data)
    },

    // Yeni şifreyi ve doğrulama kodunu onaylatır
    resetPassword(data: { email: string; code: string; newPassword: string; confirmPassword: string }) {
      return api.post('/api/Auth/reset-password', data)
    }
  }
}