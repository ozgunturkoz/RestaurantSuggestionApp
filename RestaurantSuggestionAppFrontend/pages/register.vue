<template>
  <div class="wrap">
    <div class="lang-switch-auth">
      <button :class="{ active: locale === 'tr' }" @click="setLocale('tr')">TR</button>
      <span>|</span>
      <button :class="{ active: locale === 'en' }" @click="setLocale('en')">EN</button>
    </div>

    <div class="brand">
      <div class="bg stripe"></div>
      <div class="road1" style="top:56%;transform:rotate(4deg);"></div>
      <div class="road2" style="left:58%;transform:rotate(-4deg);"></div>
      <div class="pin gold" style="left:40%;top:40%;">₺112 <span style="font-size:10px;">%88</span></div>
      <div>
        <div class="logo">Sofra</div>
        <div class="sub">{{ t('login.sub') }}</div>
      </div>
      <div>
        <div class="tagline">{{ t('register.title') }}</div>
        <div class="tagsub">{{ t('register.lead') }}</div>
      </div>
    </div>

    <div class="pane">
      <form v-if="step === 1" class="card" @submit.prevent="handleRegister">
        <h1>{{ t('register.title') }}</h1>
        <p class="lead">{{ t('register.lead') }}</p>

        <label>{{ t('register.name') }}</label>
        <input v-model="name" type="text" :placeholder="t('register.namePlaceholder')" required />

        <label>{{ t('register.email') }}</label>
        <input v-model="email" type="email" :placeholder="t('register.emailPlaceholder')" required />

        <label>{{ t('register.password') }}</label>
        <input v-model="password" type="password" placeholder="••••••••" required />
        <span style="font-size: 11px; color: #A79483; margin-top: -6px; margin-bottom: 8px; display: block;">{{ t('register.passwordHint') }}</span>

        <label>{{ t('register.confirmPassword') }}</label>
        <input v-model="confirmPassword" type="password" placeholder="••••••••" required />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;margin-top:12px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? 'Kayıt Yapılıyor...' : t('register.submit') }}</span>
        </button>

        <p class="foot">{{ t('register.hasAccount') }} <NuxtLink to="/login" class="link">{{ t('register.loginLink') }}</NuxtLink></p>
      </form>

      <form v-else-if="step === 2" class="card" @submit.prevent="handleVerify">
        <h1>Hesabınızı Doğrulayın</h1>
        <p class="lead"><b>{{ email }}</b> adresinize doğrulama kodunuz gönderildi.</p>

        <label>6 Haneli Doğrulama Kodu</label>
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
          <span>{{ loading ? 'Doğrulanıyor...' : 'Hesabı Etkinleştir ve Giriş Yap' }}</span>
        </button>

        <button 
          type="button" 
          style="background: none; border: none; color: #A79483; margin-top: 14px; cursor: pointer; font-weight: 600;" 
          @click="step = 1; errorMsg = ''"
        >
          ← Kayıt Ekranına Dön
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from '#app'
import '~/assets/auth.css'

const { locale, setLocale, t } = useI18n()
const step = ref(1)
const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const verificationCode = ref('')
const errorMsg = ref('')
const loading = ref(false)

const authStore = useAuthStore()
const router = useRouter()

const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/

const handleRegister = async () => {
  errorMsg.value = ''

  if (!emailRegex.test(email.value.trim())) {
    errorMsg.value = t('register.invalidEmail')
    return
  }

  if (password.value.length < 6) {
    errorMsg.value = t('register.passwordTooShort')
    return
  }

  if (password.value !== confirmPassword.value) {
    errorMsg.value = t('register.passwordsDoNotMatch')
    return
  }

  loading.value = true

  try {
    const res = await authStore.register(name.value, email.value, password.value, confirmPassword.value)

    if (res.requiresVerification) {
      step.value = 2
    } else if (res.success) {
      router.push('/')
    } else {
      errorMsg.value = res.error || t('register.errorMsg')
    }
  } finally {
    loading.value = false
  }
}

const handleVerify = async () => {
  errorMsg.value = ''
  loading.value = true

  try {
    const res = await authStore.verifyRegister(email.value, verificationCode.value)
    if (res.success) {
      router.push('/')
    } else {
      errorMsg.value = res.error || 'Girdiğiniz doğrulama kodu hatalı!'
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
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
</style>
