<template>
  <div class="wrap">
    <!-- TR / EN DİL BUTONU -->
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
      <form class="card" @submit.prevent="handleLogin">
        <h1>{{ t('login.welcome') }}</h1>
        <p class="lead">{{ t('login.lead') }}</p>

        <label>{{ t('login.email') }}</label>
        <input v-model="email" type="email" :placeholder="t('login.emailPlaceholder')" required />

        <label>{{ t('login.password') }}</label>
        <input v-model="password" type="password" placeholder="••••••••" required />

        <p v-if="errorMsg" class="error">{{ errorMsg }}</p>

        <button class="primary" type="submit">{{ t('login.submit') }}</button>

        <p class="foot">{{ t('login.noAccount') }} <NuxtLink to="/register" class="link">{{ t('login.registerLink') }}</NuxtLink></p>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '~/Stores/authStore'
import { useRouter } from '#app'
import '~/assets/auth.css'

const { locale, setLocale, t } = useI18n()
const email = ref('')
const password = ref('')
const errorMsg = ref('')

const authStore = useAuthStore()
const router = useRouter()

const handleLogin = async () => {
  errorMsg.value = '' 
  const success = await authStore.login(email.value, password.value)
  
  if (success) {
    router.push('/') 
  } else {
    errorMsg.value = t('login.errorMsg')
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