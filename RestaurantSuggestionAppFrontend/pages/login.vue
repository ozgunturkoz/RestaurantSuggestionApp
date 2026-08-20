<template>
  <div class="wrap">
    <div class="lang-switch-auth">
      <button :class="{ active: locale === 'tr' }" @click="setLocale('tr')">TR</button>
      <span>|</span>
      <button :class="{ active: locale === 'en' }" @click="setLocale('en')">EN</button>
    </div>

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

    <div class="pane">
      <form v-if="step === 1" class="card" @submit.prevent="handleLogin">
        <h1>{{ t('login.welcome') }}</h1>
        <p class="lead">{{ t('login.lead') }}</p>

        <label>{{ t('login.email') }}</label>
        <input v-model="email" type="email" :placeholder="t('login.emailPlaceholder')" required />

        <label>{{ t('login.password') }}</label>
        <input v-model="password" type="password" placeholder="••••••••" required />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit" :disabled="loading" style="display:flex;align-items:center;justify-content:center;gap:8px;">
          <LoadingSpinner v-if="loading" size="sm" inline />
          <span>{{ loading ? 'Kod Gönderiliyor...' : t('login.submit') }}</span>
        </button>

        <p class="foot">{{ t('login.noAccount') }} <NuxtLink to="/register" class="link">{{ t('login.registerLink') }}</NuxtLink></p>
      </form>

      <form v-else-if="step === 2" class="card" @submit.prevent="handleVerify">
        <h1>Doğrulama Kodu</h1>
        <p class="lead"><b>{{ email }}</b> adresinize gönderilen 6 haneli doğrulama kodunu giriniz.</p>

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
          <span>{{ loading ? 'Doğrulanıyor...' : 'Doğrula ve Giriş Yap' }}</span>
        </button>

        <button 
          type="button" 
          style="background: none; border: none; color: #A79483; margin-top: 14px; cursor: pointer; font-weight: 600;" 
          @click="step = 1; errorMsg = ''"
        >
          ← Giriş Ekranına Dön
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
const email = ref('')
const password = ref('')
const verificationCode = ref('')
const errorMsg = ref('')
const loading = ref(false)

const authStore = useAuthStore()
const router = useRouter()

const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/

const handleLogin = async () => {
  errorMsg.value = ''

  if (!emailRegex.test(email.value.trim())) {
    errorMsg.value = t('register.invalidEmail')
    return
  }

  loading.value = true
  
  try {
    const res = await authStore.login(email.value, password.value)
    
    if (res.requiresVerification) {
      step.value = 2
    } else if (res.success) {
      router.push('/')
    } else {
      errorMsg.value = res.error || t('login.errorMsg')
    }
  } finally {
    loading.value = false
  }
}

const handleVerify = async () => {
  errorMsg.value = ''
  loading.value = true
  
  try {
    const res = await authStore.verifyLogin(email.value, verificationCode.value)
    if (res.success) {
      router.push('/')
    } else {
      errorMsg.value = res.error || 'Girdiğiniz kod hatalı veya süresi dolmuş!'
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