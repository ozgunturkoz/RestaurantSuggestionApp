<template>
  <div class="sofra-profile-page">
    <!-- Header -->
    <header class="header">
      <div class="hbar">
        <div class="header-left">
          <NuxtLink to="/" class="logo">Sofra</NuxtLink>
          <nav>
            <NuxtLink to="/" class="nav-link" :class="{ on: $route.path === '/' }" @click.prevent="navigateTo('/')">{{ t('nav.discover') }}</NuxtLink>
            <NuxtLink to="/profile" class="nav-link" :class="{ on: $route.path === '/profile' }">{{ t('profile.myProfile') }}</NuxtLink>
          </nav>
        </div>

        <div class="header-right">
          <div class="lang-switch">
            <button :class="{ active: locale === 'tr' }" @click="setLocale('tr')">TR</button>
            <span>|</span>
            <button :class="{ active: locale === 'en' }" @click="setLocale('en')">EN</button>
          </div>
          <button class="logout-btn" @click="handleLogout">{{ t('nav.logout') }}</button>
        </div>
      </div>
    </header>

    <!-- Main Container -->
    <main class="profile-container">
      <!-- Profile Header Banner -->
      <div class="profile-banner-card">
        <div class="user-avatar">
          {{ userInitials }}
        </div>
        <div class="user-details">
          <div class="user-name-row">
            <h1>{{ authStore.user?.name || 'Kullanıcı' }}</h1>
            <span class="user-badge">Doğrulanmış Hesabı</span>
          </div>
          <p class="user-email">{{ authStore.user?.email || '' }}</p>
        </div>
      </div>

      <!-- Navigation Tabs -->
      <div class="profile-tabs">
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'expenditures' }"
          @click="activeTab = 'expenditures'"
        >
          Harcamalarım & Analiz
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'info' }"
          @click="activeTab = 'info'"
        >
          Profil Bilgileri
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'email-change' }"
          @click="activeTab = 'email-change'"
        >
          E-Posta Değiştirme
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'security' }"
          @click="activeTab = 'security'"
        >
          Şifre Değiştirme
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'favorites' }"
          @click="activeTab = 'favorites'"
        >
          Favorilerim
        </button>
      </div>

      <!-- TAB 1: HARCAMALARIM VE GRAFİKLER -->
      <div v-if="activeTab === 'expenditures'" class="tab-content">
        <!-- İstatistik Özet Kartları -->
        <div class="stats-grid">
          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">Toplam Harcama</span>
              <strong class="stat-value">₺{{ totalSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">Son 30 Gün Harcaması</span>
              <strong class="stat-value">₺{{ last30DaysSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">Ortalama Öğün Harcaması</span>
              <strong class="stat-value">₺{{ avgSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">En Çok Harcanan Kategori</span>
              <strong class="stat-value highlight-text">{{ topCategory }}</strong>
            </div>
          </div>
        </div>

        <!-- HARCAMA GRAFİKLERİ VE ANALİZ -->
        <div class="analytics-grid" v-if="expenditures.length > 0">
          <!-- Aylık / Zaman Çizelgesi Harcama Grafiği -->
          <div class="chart-card">
            <div class="chart-header">
              <h3>Aylık Harcama Detayı</h3>
              <span class="chart-sub">Dönem bazlı harcama dağılımı</span>
            </div>
            <div class="bar-chart-container">
              <div
                v-for="bar in monthlyChartData"
                :key="bar.label"
                class="bar-item"
              >
                <div class="bar-value-label">₺{{ bar.total.toFixed(0) }}</div>
                <div class="bar-wrapper">
                  <div
                    class="bar-fill"
                    :style="{ height: bar.percentage + '%' }"
                    :title="bar.label + ': ₺' + bar.total.toFixed(2)"
                  ></div>
                </div>
                <span class="bar-title">{{ bar.label }}</span>
              </div>
            </div>
          </div>

          <!-- Kategori Bazlı Dağılım -->
          <div class="chart-card">
            <div class="chart-header">
              <h3>Kategori Dağılımı</h3>
              <span class="chart-sub">Harcanan bütçenin restoran türlerine göre oranı</span>
            </div>
            <div class="category-distribution-list">
              <div
                v-for="cat in categoryChartData"
                :key="cat.name"
                class="category-bar-row"
              >
                <div class="cat-label-row">
                  <span class="cat-name">{{ cat.name }}</span>
                  <span class="cat-amount">₺{{ cat.total.toFixed(2) }} (%{{ cat.percentage.toFixed(1) }})</span>
                </div>
                <div class="cat-progress-track">
                  <div
                    class="cat-progress-fill"
                    :style="{ width: cat.percentage + '%' }"
                  ></div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Harcama Geçmişi Listesi (Okuma Modu) -->
        <div class="section-card">
          <div class="section-header">
            <div>
              <h2>Harcama Geçmişim</h2>
              <p class="section-sub">Sistemde kayıtlı otomatik hesap hareketleriniz ve dökümünüz</p>
            </div>
          </div>

          <div v-if="isLoadingExpenditures" class="loading-state">
            <LoadingSpinner size="md" />
            <p>Harcama dökümü yükleniyor...</p>
          </div>

          <div v-else-if="expenditures.length === 0" class="empty-state">
            <h3>Henüz Kayıtlı Harcamanız Bulunmamaktadır</h3>
            <p>Sistem üzerinden yaptığınız restoran işlemleri burada otomatik olarak listelenir ve analiz edilir.</p>
          </div>

          <div v-else class="expenditures-list">
            <div
              v-for="item in expenditures"
              :key="item.id"
              class="expenditure-row"
            >
              <div class="expenditure-details">
                <strong class="exp-name">{{ item.restaurantName || 'Restoran Harcaması' }}</strong>
                <div class="exp-meta">
                  <span class="category-chip">{{ item.restaurantCategory || 'Genel' }}</span>
                  <span class="exp-date">{{ formatDate(item.spentAt) }}</span>
                </div>
              </div>
              <div class="expenditure-right">
                <span class="exp-amount">₺{{ item.amount.toFixed(2) }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- TAB 2: PROFİL BİLGİLERİ -->
      <div v-if="activeTab === 'info'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>Profil Bilgilerini Güncelle</h2>
          <p class="section-sub">Hesabınızda görünen ad ve soyadı güncelleyebilirsiniz.</p>

          <form class="profile-form" @submit.prevent="handleUpdateProfile">
            <div class="form-group">
              <label for="profile-name">Ad Soyad</label>
              <input
                id="profile-name"
                v-model="profileForm.name"
                type="text"
                class="form-input"
                placeholder="Adınız Soyadınız"
                required
              />
            </div>

            <div class="form-group">
              <label>Mevcut E-posta Adresi</label>
              <input
                type="email"
                :value="authStore.user?.email"
                class="form-input readonly-input"
                disabled
              />
              <small class="field-hint">E-posta adresinizi değiştirmek için "E-Posta Değiştirme" sekmesini kullanabilirsiniz.</small>
            </div>

            <button type="submit" class="primary-btn" :disabled="isUpdatingProfile">
              {{ isUpdatingProfile ? 'Kaydediliyor...' : 'Değişiklikleri Kaydet' }}
            </button>
          </form>
        </div>
      </div>

      <!-- TAB 3: E-POSTA DEĞİŞTİRME (KOD + ŞİFRE ONAYLI + TEKRAR GİRİŞ) -->
      <div v-if="activeTab === 'email-change'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>Güvenli E-Posta Değişikliği</h2>
          <p class="section-sub">
            E-posta adresinizi değiştirmek için yeni adresinize gönderilecek doğrulama kodunu ve mevcut şifrenizi girmeniz gerekmektedir. Değişiklik sonrası hesabınıza tekrar giriş yapmanız istenecektir.
          </p>

          <!-- Step 1: Yeni E-Posta Gir ve Kod İste -->
          <div v-if="!emailChangeStepCodeSent" class="security-step-box">
            <div class="form-group">
              <label for="new-email-input">Yeni E-Posta Adresi</label>
              <input
                id="new-email-input"
                v-model="emailChangeForm.newEmail"
                type="email"
                class="form-input"
                placeholder="yeni.eposta@ornek.com"
                required
              />
            </div>

            <button
              class="primary-btn wide-btn margin-top"
              :disabled="isSendingEmailCode"
              @click="handleSendEmailChangeCode"
            >
              {{ isSendingEmailCode ? 'Doğrulama Kodu Gönderiliyor...' : 'Yeni E-Postaya Doğrulama Kodu Gönder' }}
            </button>
          </div>

          <!-- Step 2: Kod + Mevcut Şifre Gir ve Onayla -->
          <form v-else class="profile-form" @submit.prevent="handleChangeEmailSubmit">
            <div class="info-alert">
              <p>Doğrulama kodu <strong>{{ emailChangeForm.newEmail }}</strong> adresine gönderildi.</p>
            </div>

            <div class="form-group">
              <label for="email-code-input">6 Haneli Doğrulama Kodu</label>
              <input
                id="email-code-input"
                v-model="emailChangeForm.code"
                type="text"
                class="form-input code-input"
                placeholder="123456"
                maxlength="6"
                required
              />
            </div>

            <div class="form-group">
              <label for="current-password-email-confirm">Güvenlik Doğrulaması İçin Mevcut Şifreniz</label>
              <input
                id="current-password-email-confirm"
                v-model="emailChangeForm.currentPassword"
                type="password"
                class="form-input"
                placeholder="Mevcut hesabınızın şifresi"
                required
              />
            </div>

            <div class="btn-group">
              <button
                type="button"
                class="secondary-btn"
                @click="emailChangeStepCodeSent = false"
              >
                Geri
              </button>
              <button
                type="submit"
                class="primary-btn"
                :disabled="isChangingEmail"
              >
                {{ isChangingEmail ? 'E-Posta Güncelleniyor...' : 'E-Postayı Değiştir ve Yeniden Giriş Yap' }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- TAB 4: ŞİFRE DEĞİŞTİRME (MAIL KONTROLÜ İLE) -->
      <div v-if="activeTab === 'security'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>E-Posta Onaylı Şifre Değiştirme</h2>
          <p class="section-sub">
            Hesap güvenliğiniz için şifre değiştirmeden önce kayıtlı e-posta adresinize 6 haneli doğrulama kodu gönderilir.
          </p>

          <!-- Step 1: Kodu Gönder -->
          <div v-if="!passwordStepCodeSent" class="security-step-box">
            <div class="info-alert">
              <p>Kayıtlı e-posta adresiniz: <strong>{{ authStore.user?.email }}</strong></p>
            </div>
            <button
              class="primary-btn wide-btn"
              :disabled="isSendingPasswordCode"
              @click="handleSendPasswordCode"
            >
              {{ isSendingPasswordCode ? 'Doğrulama Kodu Gönderiliyor...' : 'E-Posta Doğrulama Kodu Gönder' }}
            </button>
          </div>

          <!-- Step 2: Kodu ve Yeni Şifreyi Gir -->
          <form v-else class="profile-form" @submit.prevent="handleChangePassword">
            <div class="success-alert">
              <p>Doğrulama kodu <strong>{{ authStore.user?.email }}</strong> adresine gönderildi. Lütfen e-postanızı kontrol edin.</p>
            </div>

            <div class="form-group">
              <label for="password-code">6 Haneli E-Posta Doğrulama Kodu</label>
              <input
                id="password-code"
                v-model="passwordForm.code"
                type="text"
                class="form-input code-input"
                placeholder="Örn: 123456"
                maxlength="6"
                required
              />
            </div>

            <div class="form-group">
              <label for="new-password">Yeni Şifre</label>
              <input
                id="new-password"
                v-model="passwordForm.newPassword"
                type="password"
                class="form-input"
                placeholder="En az 6 karakter"
                required
              />
            </div>

            <div class="form-group">
              <label for="confirm-password">Yeni Şifre (Tekrar)</label>
              <input
                id="confirm-password"
                v-model="passwordForm.confirmPassword"
                type="password"
                class="form-input"
                placeholder="Şifrenizi tekrar giriniz"
                required
              />
            </div>

            <div class="btn-group">
              <button
                type="button"
                class="secondary-btn"
                @click="passwordStepCodeSent = false"
              >
                Geri
              </button>
              <button
                type="submit"
                class="primary-btn"
                :disabled="isChangingPassword"
              >
                {{ isChangingPassword ? 'Şifre Güncelleniyor...' : 'Şifreyi Güncelle' }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- TAB 5: FAVORİLERİM (RESTO'YA TIKLAYINCA KEŞFET'E GİT) -->
      <div v-if="activeTab === 'favorites'" class="tab-content">
        <div class="section-card">
          <h2>Favori Restoranlarım</h2>
          <p class="section-sub">Kaydettiğiniz mekanlar. Tıklayarak Keşfet haritasında görüntüleyebilirsiniz.</p>

          <div v-if="isLoadingFavorites" class="loading-state">
            <LoadingSpinner size="md" />
            <p>Favoriler yükleniyor...</p>
          </div>

          <div v-else-if="favorites.length === 0" class="empty-state">
            <h3>Henüz Favori Restoranınız Yok</h3>
            <p>Keşfet sayfasından beğendiğiniz restoranları favorilerinize ekleyebilirsiniz.</p>
          </div>

          <div v-else class="favorites-grid">
            <div
              v-for="fav in favorites"
              :key="fav.id"
              class="favorite-card clickable-card"
              @click="goToDiscoverWithRestaurant(fav.restaurantId)"
            >
              <div class="fav-card-header">
                <h3>{{ fav.restaurant?.name || 'Restoran' }}</h3>
                <span class="category-chip">{{ fav.restaurant?.category || 'Genel' }}</span>
              </div>
              <p v-if="fav.customNote" class="fav-note">"{{ fav.customNote }}"</p>
              <div class="fav-card-footer">
                <span class="fav-price">Ort. ₺{{ fav.restaurant?.averagePrice }}</span>
                <div class="fav-actions">
                  <button
                    class="view-discover-btn"
                    @click.stop="goToDiscoverWithRestaurant(fav.restaurantId)"
                  >
                    Keşfet'te Gör
                  </button>
                  <button
                    class="remove-fav-btn"
                    @click.stop="handleRemoveFavorite(fav.restaurantId)"
                  >
                    Çıkar
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useUserService } from '~/composables/useUserService'
import { useI18n } from '~/composables/useI18n'
import { useToast } from '~/composables/useToast'
import { navigateTo } from '#app'

const authStore = useAuthStore()
const userService = useUserService()
const { t, locale, setLocale } = useI18n()
const toast = useToast()

const activeTab = ref<'expenditures' | 'info' | 'email-change' | 'security' | 'favorites'>('expenditures')

// State
const expenditures = ref<any[]>([])
const favorites = ref<any[]>([])
const isLoadingExpenditures = ref(false)
const isLoadingFavorites = ref(false)

// Profile Form State
const isUpdatingProfile = ref(false)
const profileForm = ref({
  name: authStore.user?.name || ''
})

// Email Change State
const isSendingEmailCode = ref(false)
const isChangingEmail = ref(false)
const emailChangeStepCodeSent = ref(false)
const emailChangeForm = ref({
  newEmail: '',
  code: '',
  currentPassword: ''
})

// Password Form State
const isSendingPasswordCode = ref(false)
const isChangingPassword = ref(false)
const passwordStepCodeSent = ref(false)
const passwordForm = ref({
  code: '',
  newPassword: '',
  confirmPassword: ''
})

// User Initials
const userInitials = computed(() => {
  const name = authStore.user?.name || 'K'
  return name
    .split(' ')
    .filter(Boolean)
    .map(n => n[0])
    .join('')
    .toUpperCase()
    .substring(0, 2) || 'K'
})

// Stat computations
const totalSpent = computed(() => {
  return expenditures.value.reduce((sum, item) => sum + (item.amount || 0), 0)
})

const last30DaysSpent = computed(() => {
  const thirtyDaysAgo = new Date()
  thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30)

  return expenditures.value
    .filter(item => new Date(item.spentAt) >= thirtyDaysAgo)
    .reduce((sum, item) => sum + (item.amount || 0), 0)
})

const avgSpent = computed(() => {
  if (expenditures.value.length === 0) return 0
  return totalSpent.value / expenditures.value.length
})

const topCategory = computed(() => {
  if (expenditures.value.length === 0) return '-'
  const counts: Record<string, number> = {}
  expenditures.value.forEach(item => {
    const cat = item.restaurantCategory || 'Genel'
    counts[cat] = (counts[cat] || 0) + item.amount
  })

  let bestCat = '-'
  let maxAmount = 0
  Object.entries(counts).forEach(([cat, amt]) => {
    if (amt > maxAmount) {
      maxAmount = amt
      bestCat = cat
    }
  })
  return bestCat
})

// Visual Chart Computations
const monthlyChartData = computed(() => {
  if (expenditures.value.length === 0) return []

  const monthMap: Record<string, number> = {}
  expenditures.value.forEach(item => {
    const d = new Date(item.spentAt)
    const monthKey = d.toLocaleDateString('tr-TR', { month: 'short', year: '2-digit' })
    monthMap[monthKey] = (monthMap[monthKey] || 0) + (item.amount || 0)
  })

  const entries = Object.entries(monthMap)
  if (entries.length === 0) return []

  const maxTotal = Math.max(...entries.map(([, total]) => total), 1)

  return entries.map(([label, total]) => ({
    label,
    total,
    percentage: Math.min(Math.round((total / maxTotal) * 100), 100)
  }))
})

const categoryChartData = computed(() => {
  if (expenditures.value.length === 0 || totalSpent.value === 0) return []

  const catMap: Record<string, number> = {}
  expenditures.value.forEach(item => {
    const cat = item.restaurantCategory || 'Genel'
    catMap[cat] = (catMap[cat] || 0) + (item.amount || 0)
  })

  return Object.entries(catMap)
    .map(([name, total]) => ({
      name,
      total,
      percentage: (total / totalSpent.value) * 100
    }))
    .sort((a, b) => b.total - a.total)
})

// Lifecycle
onMounted(() => {
  fetchExpenditures()
  fetchFavorites()
})

// API Calls
async function fetchExpenditures() {
  isLoadingExpenditures.value = true
  try {
    const res: any = await userService.getExpenditures()
    if (res?.isSuccess) {
      expenditures.value = res.data || []
    }
  } catch (err: any) {
    console.error('Harcamalar yüklenirken hata:', err)
  } finally {
    isLoadingExpenditures.value = false
  }
}

async function fetchFavorites() {
  isLoadingFavorites.value = true
  try {
    const res: any = await userService.getFavorites()
    if (res?.isSuccess) {
      favorites.value = res.data || []
    }
  } catch (err: any) {
    console.error('Favoriler getirilemedi:', err)
  } finally {
    isLoadingFavorites.value = false
  }
}

function goToDiscoverWithRestaurant(restaurantId: number) {
  navigateTo(`/?restaurantId=${restaurantId}`)
}

async function handleUpdateProfile() {
  if (!profileForm.value.name || profileForm.value.name.trim().length < 3) {
    toast.warning('İsim en az 3 karakter olmalıdır.')
    return
  }

  isUpdatingProfile.value = true
  try {
    const res: any = await userService.updateProfile({ name: profileForm.value.name.trim() })
    if (res?.isSuccess) {
      authStore.updateUserName(profileForm.value.name.trim())
      toast.success('Profil isminiz güncellendi.')
    } else {
      toast.error(res?.message || 'Profil güncellenemedi.')
    }
  } catch (err: any) {
    toast.error(err?.data?.message || 'Profil güncellenirken hata oluştu.')
  } finally {
    isUpdatingProfile.value = false
  }
}

async function handleSendEmailChangeCode() {
  if (!emailChangeForm.value.newEmail || !emailChangeForm.value.newEmail.includes('@')) {
    toast.warning('Lütfen geçerli bir yeni e-posta adresi giriniz.')
    return
  }

  isSendingEmailCode.value = true
  try {
    const res: any = await userService.sendEmailChangeCode({ newEmail: emailChangeForm.value.newEmail.trim() })
    if (res?.isSuccess) {
      toast.success('Doğrulama kodu yeni e-posta adresinize gönderildi.')
      emailChangeStepCodeSent.value = true
    } else {
      toast.error(res?.message || 'Kod gönderilemedi.')
    }
  } catch (err: any) {
    toast.error(err?.data?.message || 'Kod gönderilirken bir hata oluştu.')
  } finally {
    isSendingEmailCode.value = false
  }
}

async function handleChangeEmailSubmit() {
  if (!emailChangeForm.value.code || emailChangeForm.value.code.length < 6) {
    toast.warning('Lütfen 6 haneli doğrulama kodunu giriniz.')
    return
  }

  if (!emailChangeForm.value.currentPassword) {
    toast.warning('Lütfen mevcut şifrenizi giriniz.')
    return
  }

  isChangingEmail.value = true
  try {
    const res: any = await userService.changeEmailWithCode({
      newEmail: emailChangeForm.value.newEmail.trim(),
      code: emailChangeForm.value.code.trim(),
      currentPassword: emailChangeForm.value.currentPassword
    })

    if (res?.isSuccess) {
      toast.success('E-posta adresiniz güncellendi! Lütfen yeni e-posta adresinizle tekrar giriş yapın.')
      authStore.logout()
    } else {
      toast.error(res?.message || 'E-posta adresi değiştirilemedi.')
    }
  } catch (err: any) {
    toast.error(err?.data?.message || 'E-posta değiştirilirken hata oluştu.')
  } finally {
    isChangingEmail.value = false
  }
}

async function handleSendPasswordCode() {
  isSendingPasswordCode.value = true
  try {
    const res: any = await userService.sendPasswordCode()
    if (res?.isSuccess) {
      toast.success('Doğrulama kodu e-postanıza gönderildi.')
      passwordStepCodeSent.value = true
    } else {
      toast.error(res?.message || 'Kod gönderilemedi.')
    }
  } catch (err: any) {
    toast.error(err?.data?.message || 'Kod gönderilirken bir hata oluştu.')
  } finally {
    isSendingPasswordCode.value = false
  }
}

async function handleChangePassword() {
  if (passwordForm.value.newPassword.length < 6) {
    toast.warning('Şifre en az 6 karakter olmalıdır.')
    return
  }

  if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
    toast.warning('Girdiğiniz şifreler eşleşmiyor.')
    return
  }

  isChangingPassword.value = true
  try {
    const res: any = await userService.changePasswordWithCode({
      code: passwordForm.value.code.trim(),
      newPassword: passwordForm.value.newPassword
    })

    if (res?.isSuccess) {
      toast.success('Şifreniz başarıyla değiştirildi!')
      passwordStepCodeSent.value = false
      passwordForm.value = { code: '', newPassword: '', confirmPassword: '' }
    } else {
      toast.error(res?.message || 'Şifre değiştirilemedi.')
    }
  } catch (err: any) {
    toast.error(err?.data?.message || 'Şifre değiştirilirken bir hata oluştu.')
  } finally {
    isChangingPassword.value = false
  }
}

async function handleRemoveFavorite(restaurantId: number) {
  try {
    const res: any = await userService.removeFavorite(restaurantId)
    if (res?.isSuccess) {
      toast.success('Favorilerden çıkarıldı.')
      favorites.value = favorites.value.filter(f => f.restaurantId !== restaurantId)
    } else {
      toast.error(res?.message || 'İşlem gerçekleştirilemedi.')
    }
  } catch (err: any) {
    toast.error('Favori çıkarılırken hata oluştu.')
  }
}

function formatDate(dateStr: string) {
  if (!dateStr) return '-'
  const date = new Date(dateStr)
  return date.toLocaleDateString('tr-TR', {
    day: '2-digit',
    month: 'long',
    year: 'numeric'
  })
}

function handleLogout() {
  authStore.logout()
}
</script>

<style scoped>
.sofra-profile-page {
  min-height: 100vh;
  background-color: #201a16;
  color: #f3e9de;
  font-family: 'Manrope', system-ui, -apple-system, sans-serif;
}

/* Header */
.header {
  background: rgba(23, 18, 15, 0.85);
  backdrop-filter: blur(12px);
  border-bottom: 1px solid #33291f;
  position: sticky;
  top: 0;
  z-index: 100;
}

.hbar {
  max-width: 1200px;
  margin: 0 auto;
  padding: 12px 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 20px;
}

.logo {
  font-family: 'Instrument Serif', serif;
  font-size: 25px;
  font-style: italic;
  color: #f3e9de;
  text-decoration: none;
}

nav {
  display: flex;
  gap: 20px;
  font-size: 14px;
  font-weight: 600;
  align-items: center;
}

.nav-link {
  color: #a79483;
  text-decoration: none;
  transition: color 0.2s;
  padding: 4px 0;
}

.nav-link:hover,
.nav-link.on {
  color: #e0a458 !important;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.lang-switch {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 11px;
  font-weight: 700;
  color: #a79483;
}

.lang-switch button {
  background: none;
  border: none;
  color: #a79483;
  cursor: pointer;
  font-weight: 700;
  padding: 2px 4px;
  border-radius: 4px;
}

.lang-switch button.active {
  color: #e0a458;
  background: rgba(224, 164, 88, 0.15);
}

.logout-btn {
  padding: 5px 12px;
  background: #40342a;
  color: #f3e9de;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-size: 11px;
  font-weight: 700;
  transition: background 0.2s;
}

.logout-btn:hover {
  background: #5a4526;
}

/* Main Container */
.profile-container {
  max-width: 1080px;
  margin: 0 auto;
  padding: 28px 24px 60px;
}

/* Banner Card */
.profile-banner-card {
  background: #2b231d;
  border: 1px solid #40342a;
  border-radius: 16px;
  padding: 24px 28px;
  display: flex;
  align-items: center;
  gap: 20px;
  box-shadow: 0 6px 16px -8px rgba(0, 0, 0, 0.6);
  margin-bottom: 24px;
}

.user-avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background: #e0a458;
  color: #17120f;
  font-size: 22px;
  font-weight: 800;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.user-details {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.user-name-row {
  display: flex;
  align-items: center;
  gap: 12px;
}

.user-name-row h1 {
  font-family: 'Instrument Serif', serif;
  font-size: 26px;
  font-weight: 400;
  margin: 0;
  color: #f3e9de;
}

.user-badge {
  background: rgba(143, 191, 126, 0.15);
  color: #8fbf7e;
  border: 1px solid rgba(143, 191, 126, 0.3);
  font-size: 11px;
  padding: 2px 8px;
  border-radius: 12px;
  font-weight: 600;
}

.user-email {
  color: #a79483;
  font-size: 13px;
  margin: 0;
}

/* Tabs */
.profile-tabs {
  display: flex;
  gap: 8px;
  border-bottom: 1px solid #33291f;
  margin-bottom: 24px;
  overflow-x: auto;
}

.tab-btn {
  background: none;
  border: none;
  color: #a79483;
  font-size: 14px;
  font-weight: 600;
  padding: 10px 18px;
  border-radius: 8px 8px 0 0;
  cursor: pointer;
  transition: all 0.2s;
  white-space: nowrap;
}

.tab-btn:hover {
  color: #f3e9de;
  background: rgba(255, 255, 255, 0.03);
}

.tab-btn.active {
  color: #e0a458;
  border-bottom: 2px solid #e0a458;
  background: rgba(224, 164, 88, 0.08);
}

/* Stats */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 14px;
  margin-bottom: 24px;
}

.stat-card {
  background: #2b231d;
  border: 1px solid #40342a;
  border-radius: 12px;
  padding: 18px;
  display: flex;
  align-items: center;
}

.stat-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.stat-label {
  font-size: 12px;
  color: #8a745e;
}

.stat-value {
  font-size: 20px;
  font-weight: 700;
  color: #e0a458;
}

.highlight-text {
  font-size: 15px;
  color: #8fbf7e;
}

/* Visual Spending Analytics Charts */
.analytics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 18px;
  margin-bottom: 24px;
}

.chart-card {
  background: #2b231d;
  border: 1px solid #40342a;
  border-radius: 14px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.chart-header h3 {
  font-family: 'Instrument Serif', serif;
  font-size: 20px;
  margin: 0 0 2px;
  color: #f3e9de;
}

.chart-sub {
  font-size: 12px;
  color: #8a745e;
}

/* Bar Chart */
.bar-chart-container {
  display: flex;
  align-items: flex-end;
  justify-content: space-around;
  height: 180px;
  padding-top: 24px;
  border-bottom: 1px solid #40342a;
  gap: 12px;
}

.bar-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  flex: 1;
  height: 100%;
}

.bar-value-label {
  font-size: 10px;
  font-weight: 700;
  color: #e0a458;
  margin-bottom: 6px;
}

.bar-wrapper {
  flex: 1;
  width: 100%;
  max-width: 36px;
  background: #201a16;
  border-radius: 6px 6px 0 0;
  display: flex;
  align-items: flex-end;
  overflow: hidden;
  border: 1px solid #33291f;
}

.bar-fill {
  width: 100%;
  background: linear-gradient(180deg, #e0a458 0%, #a26e2e 100%);
  border-radius: 4px 4px 0 0;
  transition: height 0.4s ease-out;
}

.bar-title {
  font-size: 11px;
  color: #a79483;
  margin-top: 8px;
  font-weight: 600;
}

/* Category Progress Bars */
.category-distribution-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.category-bar-row {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.cat-label-row {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
}

.cat-name {
  color: #f3e9de;
  font-weight: 600;
}

.cat-amount {
  color: #a79483;
  font-size: 11px;
}

.cat-progress-track {
  height: 8px;
  background: #201a16;
  border-radius: 4px;
  overflow: hidden;
  border: 1px solid #33291f;
}

.cat-progress-fill {
  height: 100%;
  background: #8fbf7e;
  border-radius: 4px;
  transition: width 0.4s ease;
}

/* Section Card */
.section-card {
  background: #2b231d;
  border: 1px solid #40342a;
  border-radius: 14px;
  padding: 24px;
}

.section-card.max-w-xl {
  max-width: 540px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
  gap: 16px;
}

.section-card h2 {
  font-family: 'Instrument Serif', serif;
  font-size: 22px;
  font-weight: 400;
  color: #f3e9de;
  margin: 0 0 4px 0;
}

.section-sub {
  font-size: 12px;
  color: #8a745e;
  margin: 0;
}

/* Expenditures List */
.expenditures-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.expenditure-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #201a16;
  border: 1px solid #40342a;
  padding: 12px 16px;
  border-radius: 10px;
}

.expenditure-details {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.exp-name {
  font-size: 14px;
  color: #f3e9de;
  font-weight: 600;
}

.exp-meta {
  display: flex;
  align-items: center;
  gap: 10px;
}

.category-chip {
  background: rgba(224, 164, 88, 0.12);
  color: #e0a458;
  font-size: 11px;
  font-weight: 600;
  padding: 2px 8px;
  border-radius: 6px;
}

.exp-date {
  font-size: 11px;
  color: #8a745e;
}

.expenditure-right {
  display: flex;
  align-items: center;
  gap: 14px;
}

.exp-amount {
  font-size: 15px;
  font-weight: 700;
  color: #8fbf7e;
}

/* Empty & Loading states */
.loading-state,
.empty-state {
  padding: 40px 20px;
  text-align: center;
  color: #8a745e;
}

.empty-state h3 {
  color: #f3e9de;
  margin: 0 0 6px;
  font-family: 'Instrument Serif', serif;
  font-size: 20px;
}

.empty-state p {
  max-width: 400px;
  margin: 0 auto;
  font-size: 13px;
}

/* Form Styles */
.profile-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  margin-top: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.form-group label {
  font-size: 12px;
  font-weight: 600;
  color: #c9b9a8;
}

.field-hint {
  font-size: 11px;
  color: #8a745e;
  margin-top: 2px;
}

.form-input {
  background: #201a16;
  border: 1px solid #40342a;
  border-radius: 8px;
  padding: 10px 12px;
  color: #f3e9de;
  font-size: 13px;
  outline: none;
  transition: border-color 0.2s;
}

.form-input:focus {
  border-color: #e0a458;
}

.readonly-input {
  background: rgba(32, 26, 22, 0.4);
  color: #8a745e;
  cursor: not-allowed;
}

.code-input {
  letter-spacing: 4px;
  font-size: 16px;
  font-weight: 700;
  text-align: center;
  color: #e0a458;
}

/* Buttons */
.primary-btn {
  background: #e0a458;
  color: #17120f;
  border: none;
  padding: 10px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.2s;
}

.primary-btn:hover:not(:disabled) {
  background: #c58434;
}

.primary-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.secondary-btn {
  background: #40342a;
  color: #f3e9de;
  border: none;
  padding: 10px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

.secondary-btn:hover {
  background: #5a4526;
}

.wide-btn {
  width: 100%;
}

.margin-top {
  margin-top: 10px;
}

.btn-group {
  display: flex;
  gap: 10px;
  justify-content: flex-end;
}

/* Alerts */
.info-alert,
.success-alert {
  padding: 12px 14px;
  border-radius: 8px;
  font-size: 13px;
  margin-bottom: 14px;
}

.info-alert {
  background: rgba(59, 130, 246, 0.12);
  border: 1px solid rgba(59, 130, 246, 0.25);
  color: #60a5fa;
}

.success-alert {
  background: rgba(34, 197, 94, 0.12);
  border: 1px solid rgba(34, 197, 94, 0.25);
  color: #4ade80;
}

/* Favorites Grid */
.favorites-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
  gap: 14px;
  margin-top: 14px;
}

.favorite-card {
  background: #201a16;
  border: 1px solid #40342a;
  border-radius: 10px;
  padding: 14px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 10px;
  transition: transform 0.2s, border-color 0.2s;
}

.clickable-card {
  cursor: pointer;
}

.clickable-card:hover {
  transform: translateY(-2px);
  border-color: #e0a458;
}

.fav-card-header h3 {
  font-size: 15px;
  margin: 0 0 4px;
  color: #f3e9de;
}

.fav-note {
  font-size: 12px;
  color: #a79483;
  font-style: italic;
  margin: 0;
}

.fav-card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 8px;
  border-top: 1px solid #33291f;
}

.fav-price {
  font-size: 12px;
  font-weight: 600;
  color: #e0a458;
}

.fav-actions {
  display: flex;
  gap: 6px;
}

.view-discover-btn {
  background: rgba(224, 164, 88, 0.15);
  color: #e0a458;
  border: 1px solid rgba(224, 164, 88, 0.3);
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.2s;
}

.view-discover-btn:hover {
  background: rgba(224, 164, 88, 0.3);
}

.remove-fav-btn {
  background: rgba(239, 68, 68, 0.12);
  color: #f87171;
  border: none;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 600;
  cursor: pointer;
}

.remove-fav-btn:hover {
  background: rgba(239, 68, 68, 0.25);
}
</style>
