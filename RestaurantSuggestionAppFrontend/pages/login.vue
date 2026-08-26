<template>
  <!-- =========================================================================================
       GİRİŞ VE ŞİFREMİ UNUTTUM SAYFASI ŞABLONU (KODLAMA BİLMEYENLER İÇİN ANLATIM)
       Bu sayfa kullanıcıların sisteme güvenli giriş yapmasını sağlar.
       İçerisinde 4 farklı adım (Step) barındırır:
       - Step 1: E-posta ve Şifre ile giriş formu (+ Şifremi Unuttum linki)
       - Step 2: 2 Aşamalı Giriş Doğrulama Kodu (2FA) formu
       - Step 3: Şifremi Unuttum - E-posta adresine sıfırlama kodu isteme formu
       - Step 4: Şifremi Unuttum - E-postaya gelen kod ile yeni şifreyi belirleme formu
       ========================================================================================= -->
  <div class="wrap">
    <!-- Sağ Üst Köşe TR / EN Dil Değiştirme Butonları -->
    <div class="lang-switch-auth">
      <button :class="{ active: locale === 'tr' }" @click="setLocale('tr')">TR</button>
      <span>|</span>
      <button :class="{ active: locale === 'en' }" @click="setLocale('en')">EN</button>
    </div>

    <!-- Sol Taraf Marka ve Görsel Tanıtım Alanı (Brand Pane) -->
    <div class="brand">
      <div class="bg stripe"></div>
      <div class="road1"></div>
      <div class="road2"></div>
      <div class="pin gold" style="left:34%;top:34%;">₺98 <span style="font-size:10px;">%94</span></div>
      <div class="pin dark" style="left:62%;top:58%;">₺89 <span style="font-size:10px;opacity:.8;">%81</span></div>
      <div>
        <div class="logo">Sofra</div>
        <div class="sub">{{ t('login.sub') }}</div>
      </div>
      <div>
        <div class="tagline">{{ t('login.taglineTitle') }}</div>
        <div class="tagsub">{{ t('login.taglineSub') }}</div>
      </div>
    </div>

    <!-- Sağ Taraf Form Alanı (Form Pane) -->
    <div class="pane">
      <!-- =====================================================================================
           ADIM 1: NORMAL GİRİŞ FORMU
           ===================================================================================== -->
      <form v-if="step === 1" class="card" @submit.prevent="handleLogin">
        <h1>{{ t('login.welcome') }}</h1>
        <p class="lead">{{ t('login.lead') }}</p>

        <label>{{ t('login.email') }}</label>
        <input v-model="email" type="email" :placeholder="t('login.emailPlaceholder')" required />

        <div class="password-label-row">
          <label>{{ t('login.password') }}</label>
          <button type="button" class="forgot-link-btn" @click="startForgotPassword">
            {{ t('forgot.forgotLink') }}
          </button>
        </div>
        <input v-model="password" type="password" placeholder="••••••••" required />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? t('login.sendingCode') : t('login.submit') }}</span>
        </button>

        <p class="foot">{{ t('login.noAccount') }} <NuxtLink to="/register" class="link">{{ t('login.registerLink') }}</NuxtLink></p>
      </form>

      <!-- =====================================================================================
           ADIM 2: GİRİŞ 2FA DOĞRULAMA KODU FORMU
           ===================================================================================== -->
      <form v-else-if="step === 2" class="card" @submit.prevent="handleVerify">
        <h1>{{ t('login.verifyTitle') }}</h1>
        <p class="lead">{{ t('login.verifyLead', { email: email }) }}</p>

        <label>{{ t('login.verifyCodeLabel') }}</label>
        <input 
          v-model="verificationCode" 
          type="text" 
          maxlength="6" 
          placeholder="123456" 
          style="letter-spacing: 6px; font-size: 20px; text-align: center; font-weight: bold;" 
          required 
        />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? t('login.verifying') : t('login.verifyBtn') }}</span>
        </button>

        <button 
          type="button" 
          class="back-step-btn" 
          @click="step = 1; errorMsg = ''"
        >
          {{ t('login.backToLogin') }}
        </button>
      </form>

      <!-- =====================================================================================
           ADIM 3: ŞİFREMİ UNUTTUM - SIFIRLAMA KODU İSTEME FORMU
           ===================================================================================== -->
      <form v-else-if="step === 3" class="card" @submit.prevent="handleRequestForgotCode">
        <h1>{{ t('forgot.title') }}</h1>
        <p class="lead">{{ t('forgot.lead') }}</p>

        <label>{{ t('forgot.email') }}</label>
        <input v-model="forgotEmail" type="email" :placeholder="t('forgot.emailPlaceholder')" required />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? t('forgot.sending') : t('forgot.sendCode') }}</span>
        </button>

        <button 
          type="button" 
          class="back-step-btn" 
          @click="step = 1; errorMsg = ''"
        >
          {{ t('forgot.backToLogin') }}
        </button>
      </form>

      <!-- =====================================================================================
           ADIM 4: ŞİFREMİ UNUTTUM - KOD VE YENİ ŞİFRE İLE SIFIRLAMA FORMU
           ===================================================================================== -->
      <form v-else-if="step === 4" class="card" @submit.prevent="handleResetPassword">
        <h1>{{ t('forgot.resetTitle') }}</h1>
        <p class="lead">{{ t('forgot.resetLead', { email: forgotEmail }) }}</p>

        <label>{{ t('forgot.code') }}</label>
        <input 
          v-model="forgotCode" 
          type="text" 
          maxlength="6" 
          :placeholder="t('forgot.codePlaceholder')" 
          style="letter-spacing: 6px; font-size: 20px; text-align: center; font-weight: bold;" 
          required 
        />

        <label>{{ t('forgot.newPassword') }}</label>
        <input 
          v-model="forgotNewPassword" 
          type="password" 
          :placeholder="t('forgot.newPasswordPlaceholder')" 
          required 
        />

        <label>{{ t('forgot.confirmPassword') }}</label>
        <input 
          v-model="forgotConfirmPassword" 
          type="password" 
          :placeholder="t('forgot.confirmPasswordPlaceholder')" 
          required 
        />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? t('forgot.resetting') : t('forgot.resetBtn') }}</span>
        </button>

        <button 
          type="button" 
          class="back-step-btn" 
          @click="step = 3; errorMsg = ''"
        >
          {{ t('forgot.backToEmailStep') }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
// =========================================================================================
// GİRİŞ SAYFASI KODLARI (JAVASCRIPT / VUE COMPOSITION API)
// =========================================================================================
import { ref } from 'vue'
import { useRouter } from '#app'

// Çoklu dil aracı
const { locale, setLocale, t } = useI18n()

// Form adımı (1: Giriş, 2: 2FA Doğrulama, 3: Şifremi Unuttum Kod İste, 4: Şifre Sıfırla)
const step = ref(1)

// Giriş Formu Değişkenleri
const email = ref('')
const password = ref('')
const verificationCode = ref('')

// Şifremi Unuttum Değişkenleri
const forgotEmail = ref('')
const forgotCode = ref('')
const forgotNewPassword = ref('')
const forgotConfirmPassword = ref('')

// Durum Değişkenleri
const errorMsg = ref('')
const loading = ref(false)

// Servis ve Bildirim Havuzları
const authStore = useAuthStore()
const authService = useAuthService()
const toast = useToast()
const router = useRouter()

// E-posta regex doğrulaması
const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/

// -----------------------------------------------------------------------------------------
// 1. ADIM: Normal Giriş Yap Butonuna Basıldığında Çalışan Fonksiyon
// -----------------------------------------------------------------------------------------
const handleLogin = async () => {
  errorMsg.value = ''
  
  if (!emailRegex.test(email.value.trim())) {
    errorMsg.value = t('register.invalidEmail')
    return
  }

  loading.value = true
  try {
    const res = await authStore.login(email.value.trim(), password.value)
    if (res.success) {
      if (res.requiresVerification) {
        step.value = 2
        toast.showInfo(t('login.verificationCodeSentToast'))
      } else {
        router.push('/')
      }
    } else {
      errorMsg.value = res.error || t('login.errorMsg')
    }
  } finally {
    loading.value = false
  }
}

// -----------------------------------------------------------------------------------------
// 2. ADIM: 2FA Doğrulama Kodunu Onaylayan Fonksiyon
// -----------------------------------------------------------------------------------------
const handleVerify = async () => {
  errorMsg.value = ''
  
  if (!verificationCode.value || verificationCode.value.trim().length !== 6) {
    errorMsg.value = t('login.enterSixDigitCode')
    return
  }

  loading.value = true
  try {
    const res = await authStore.verifyLogin(email.value.trim(), verificationCode.value.trim())
    if (res.success) {
      router.push('/')
    } else {
      errorMsg.value = res.error || t('login.invalidCode')
    }
  } finally {
    loading.value = false
  }
}

// -----------------------------------------------------------------------------------------
// 3. ADIM: Şifremi Unuttum Akışını Başlatan Fonksiyon
// -----------------------------------------------------------------------------------------
const startForgotPassword = () => {
  errorMsg.value = ''
  forgotEmail.value = email.value.trim()
  step.value = 3
}

// -----------------------------------------------------------------------------------------
// 4. ADIM: Şifre Sıfırlama Kodu İsteyen Fonksiyon
// -----------------------------------------------------------------------------------------
const handleRequestForgotCode = async () => {
  errorMsg.value = ''
  
  if (!emailRegex.test(forgotEmail.value.trim())) {
    errorMsg.value = t('register.invalidEmail')
    return
  }

  loading.value = true
  try {
    const res = await authService.forgotPassword(forgotEmail.value.trim())
    if (res.success) {
      toast.showSuccess(t('forgot.codeSentToast'))
      step.value = 4
    } else {
      errorMsg.value = res.error || t('forgot.codeSendError')
    }
  } finally {
    loading.value = false
  }
}

// -----------------------------------------------------------------------------------------
// 5. ADIM: Kod ve Yeni Şifre ile Sıfırlamayı Tamamlayan Fonksiyon
// -----------------------------------------------------------------------------------------
const handleResetPassword = async () => {
  errorMsg.value = ''

  if (!forgotCode.value || forgotCode.value.trim().length !== 6) {
    errorMsg.value = t('login.enterSixDigitCode')
    return
  }

  if (forgotNewPassword.value.length < 6) {
    errorMsg.value = t('profile.passwordMinLength')
    return
  }

  if (forgotNewPassword.value !== forgotConfirmPassword.value) {
    errorMsg.value = t('register.passwordsDoNotMatch')
    return
  }

  loading.value = true
  try {
    const res = await authService.resetPassword(
      forgotEmail.value.trim(),
      forgotCode.value.trim(),
      forgotNewPassword.value,
      forgotConfirmPassword.value
    )

    if (res.success) {
      toast.showSuccess(t('forgot.resetSuccessToast'))
      email.value = forgotEmail.value
      password.value = ''
      step.value = 1
    } else {
      errorMsg.value = res.error || t('profile.passwordChangeFailed')
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* =========================================================================================
   GİRİŞ VE AUTH SAYFASI ÖZEL STİLLERİ (SCOPED CSS)
   ========================================================================================= */
.wrap {
  min-height: 100vh;
  display: grid;
  grid-template-columns: 1.05fr 1fr;
  font-family: 'Manrope', sans-serif;
  background: #17120F;
  color: #F3E9DE;
  position: relative;
}

.brand {
  position: relative;
  background: #17120F;
  border-right: 1px solid #33291F;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 44px 48px;
}

.brand .bg {
  position: absolute;
  inset: 0;
  background: #1C1611;
}

.brand .bg.stripe {
  background-image: repeating-linear-gradient(135deg, rgba(255,255,255,.035) 0 9px, rgba(255,255,255,0) 9px 18px);
}

.brand .road1 {
  position: absolute;
  left: -4%;
  top: 40%;
  width: 108%;
  height: 14px;
  background: #241E18;
  transform: rotate(-6deg);
}

.brand .road2 {
  position: absolute;
  left: 44%;
  top: -4%;
  width: 11px;
  height: 108%;
  background: #241E18;
  transform: rotate(4deg);
}

.pin {
  position: absolute;
  display: flex;
  align-items: center;
  gap: 5px;
  padding: 6px 11px;
  border-radius: 18px;
  font-size: 12px;
  font-weight: 800;
  z-index: 2;
}

.pin.gold {
  background: #E0A458;
  color: #201A16;
  box-shadow: 0 8px 20px -6px rgba(224,164,88,.5);
}

.pin.dark {
  background: #2B231D;
  color: #F3E9DE;
  border: 1px solid #5A4526;
}

.logo {
  font-family: 'Instrument Serif', serif;
  font-size: 30px;
  font-style: italic;
  position: relative;
  z-index: 2;
}

.sub {
  font-size: 12px;
  color: #A79483;
  letter-spacing: .05em;
  margin-top: 2px;
  position: relative;
  z-index: 2;
}

.tagline {
  font-family: 'Instrument Serif', serif;
  font-size: 30px;
  line-height: 1.2;
  position: relative;
  z-index: 2;
}

.tagsub {
  font-size: 13.5px;
  color: #8A745E;
  margin-top: 14px;
  position: relative;
  z-index: 2;
}

.pane {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 48px;
  background: #201A16;
}

.card {
  width: 100%;
  max-width: 380px;
}

h1 {
  font-family: 'Instrument Serif', serif;
  font-size: 32px;
  font-weight: 400;
  margin: 0 0 6px;
  color: #F3E9DE;
}

.lead {
  font-size: 14px;
  color: #A79483;
  margin: 0 0 28px;
}

label {
  display: block;
  font-size: 12.5px;
  color: #C9B9A8;
  font-weight: 600;
  margin-bottom: 7px;
}

input {
  width: 100%;
  background: #2B231D;
  border: 1px solid #40342A;
  border-radius: 12px;
  padding: 13px 15px;
  color: #F3E9DE;
  font-size: 14px;
  font-family: 'Manrope', sans-serif;
  outline: none;
  margin-bottom: 15px;
  box-sizing: border-box;
}

input:focus {
  border-color: #E0A458;
}

.primary {
  width: 100%;
  background: #E0A458;
  color: #201A16;
  border: none;
  padding: 15px;
  border-radius: 13px;
  font-size: 15px;
  font-weight: 800;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  transition: opacity .15s;
}

.primary:hover {
  opacity: 0.9;
}

.primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.error {
  color: #E0716A;
  font-size: 13.5px;
  margin: 0 0 15px;
  text-align: center;
  font-weight: 600;
}

.foot {
  text-align: center;
  font-size: 13.5px;
  color: #A79483;
  margin-top: 26px;
}

.link {
  color: #E0A458;
  font-weight: 700;
  cursor: pointer;
  text-decoration: none;
}

.lang-switch-auth {
  position: absolute;
  top: 20px;
  right: 20px;
  z-index: 10;
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 13px;
  font-weight: 700;
  color: #A79483;
  background: #2B231D;
  padding: 6px 12px;
  border-radius: 10px;
  border: 1px solid #40342A;
}

.lang-switch-auth button {
  background: none;
  border: none;
  color: #A79483;
  cursor: pointer;
  font-weight: 700;
  font-family: 'Manrope', sans-serif;
  padding: 2px 6px;
  border-radius: 4px;
  transition: color .2s;
}

.lang-switch-auth button.active {
  color: #E0A458;
  background: rgba(224, 164, 88, 0.15);
}

.lang-switch-auth button:hover {
  color: #F3E9DE;
}

.password-label-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 4px;
}

.forgot-link-btn {
  background: none;
  border: none;
  color: #E0A458;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  padding: 0;
  text-decoration: underline;
  transition: color 0.2s;
}

.forgot-link-btn:hover {
  color: #F3E9DE;
}

.back-step-btn {
  background: none;
  border: none;
  color: #A79483;
  margin-top: 14px;
  cursor: pointer;
  font-weight: 600;
  font-size: 13px;
  transition: color 0.2s;
}

.back-step-btn:hover {
  color: #F3E9DE;
}

@media (max-width: 820px) {
  .wrap { grid-template-columns: 1fr; }
  .brand { display: none; }
  .pane { padding: 32px 20px; }
}
</style>