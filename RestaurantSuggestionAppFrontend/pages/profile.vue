<template>

  <div class="sofra-profile-page">

    <header class="header">
      <div class="hbar">
        <div class="header-left">
          <NuxtLink to="/" class="logo">Sofra</NuxtLink>
          <nav>

            <NuxtLink to="/" class="nav-link" :class="{ on: $route.path === '/' }">{{ t('nav.discover') }}</NuxtLink>

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

    <main class="profile-container">

      <div class="profile-banner-card">
        <div class="user-avatar">
          {{ userInitials }}
        </div>
        <div class="user-details">
          <div class="user-name-row">
            <h1>{{ authStore.user?.name || t('profile.defaultUser') }}</h1>
            <span class="user-badge">{{ t('profile.verifiedAccount') }}</span>
          </div>
          <p class="user-email">{{ authStore.user?.email || '' }}</p>
        </div>
      </div>

      <div class="profile-tabs">
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'expenditures' }"
          @click="activeTab = 'expenditures'"
        >
          {{ t('profile.tabExpenditures') }}
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'info' }"
          @click="activeTab = 'info'"
        >
          {{ t('profile.tabInfo') }}
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'email-change' }"
          @click="activeTab = 'email-change'"
        >
          {{ t('profile.tabEmailChange') }}
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'security' }"
          @click="activeTab = 'security'"
        >
          {{ t('profile.tabSecurity') }}
        </button>
        <button
          class="tab-btn"
          :class="{ active: activeTab === 'favorites' }"
          @click="activeTab = 'favorites'"
        >
          {{ t('profile.tabFavorites') }}
        </button>
      </div>

      <div v-if="activeTab === 'expenditures'" class="tab-content">

        <div class="stats-grid">
          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">{{ t('profile.totalSpent') }}</span>
              <strong class="stat-value">₺{{ totalSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">{{ t('profile.last30DaysSpent') }}</span>
              <strong class="stat-value">₺{{ last30DaysSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">{{ t('profile.avgSpent') }}</span>
              <strong class="stat-value">₺{{ avgSpent.toFixed(2) }}</strong>
            </div>
          </div>

          <div class="stat-card">
            <div class="stat-info">
              <span class="stat-label">{{ t('profile.topCategory') }}</span>
              <strong class="stat-value highlight-text">{{ topCategory }}</strong>
            </div>
          </div>
        </div>

        <div class="analytics-grid" v-if="expenditures.length > 0">

          <div class="chart-card">
            <div class="chart-header">
              <h3>{{ t('profile.monthlyDetail') }}</h3>
              <span class="chart-sub">{{ t('profile.monthlyDetailSub') }}</span>
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

          <div class="chart-card">
            <div class="chart-header">
              <h3>{{ t('profile.categoryDistribution') }}</h3>
              <span class="chart-sub">{{ t('profile.categoryDistributionSub') }}</span>
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

        <div class="section-card">
          <div class="section-header">
            <div>
              <h2>{{ t('profile.expendituresHistory') }}</h2>
              <p class="section-sub">{{ t('profile.expendituresHistorySub') }}</p>
            </div>
          </div>

          <div v-if="isLoadingExpenditures" class="loading-state">
            <LoadingSpinner size="md" />
            <p>{{ t('profile.loadingExpenditures') }}</p>
          </div>

          <div v-else-if="expenditures.length === 0" class="empty-state">
            <h3>{{ t('profile.noExpenditures') }}</h3>
            <p>{{ t('profile.noExpendituresSub') }}</p>
          </div>

          <div v-else class="expenditures-list">
            <div
              v-for="item in expenditures"
              :key="item.id"
              class="expenditure-row"
            >
              <div class="expenditure-details">
                <strong class="exp-name">{{ item.restaurantName || t('profile.defaultExpenditureName') }}</strong>
                <div class="exp-meta">
                  <span class="category-chip">{{ item.restaurantCategory || t('profile.defaultCategory') }}</span>
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

      <div v-if="activeTab === 'info'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>{{ t('profile.updateProfileTitle') }}</h2>
          <p class="section-sub">{{ t('profile.updateProfileSub') }}</p>

          <form class="profile-form" @submit.prevent="handleUpdateProfile">
            <div class="form-group">
              <label for="profile-name">{{ t('profile.fullName') }}</label>
              <input
                id="profile-name"
                v-model="profileForm.name"
                type="text"
                class="form-input"
                :placeholder="t('profile.fullNamePlaceholder')"
                required
              />
            </div>

            <div class="form-group">
              <label>{{ t('profile.currentEmail') }}</label>
              <input
                type="email"
                :value="authStore.user?.email"
                class="form-input readonly-input"
                disabled
              />
              <small class="field-hint">{{ t('profile.emailHint') }}</small>
            </div>

            <button type="submit" class="primary-btn" :disabled="isUpdatingProfile">
              {{ isUpdatingProfile ? t('profile.saving') : t('profile.saveChanges') }}
            </button>
          </form>
        </div>
      </div>

      <div v-if="activeTab === 'email-change'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>{{ t('profile.emailChangeTitle') }}</h2>
          <p class="section-sub">{{ t('profile.emailChangeSub') }}</p>

          <div v-if="!emailChangeStepCodeSent" class="security-step-box">
            <div class="form-group">
              <label for="new-email-input">{{ t('profile.newEmailLabel') }}</label>
              <input
                id="new-email-input"
                v-model="emailChangeForm.newEmail"
                type="email"
                class="form-input"
                :placeholder="t('profile.newEmailPlaceholder')"
                required
              />
            </div>

            <button
              class="primary-btn wide-btn margin-top"
              :disabled="isSendingEmailCode"
              @click="handleSendEmailChangeCode"
            >
              {{ isSendingEmailCode ? t('profile.sendingCode') : t('profile.sendCodeToNewEmail') }}
            </button>
          </div>

          <form v-else class="profile-form" @submit.prevent="handleChangeEmailSubmit">
            <div class="info-alert">
              <p>{{ t('profile.codeSentToNewEmail', { email: emailChangeForm.newEmail }) }}</p>
            </div>

            <div class="form-group">
              <label for="email-code-input">{{ t('profile.sixDigitCode') }}</label>
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
              <label for="current-password-email-confirm">{{ t('profile.currentPasswordForSecurity') }}</label>
              <input
                id="current-password-email-confirm"
                v-model="emailChangeForm.currentPassword"
                type="password"
                class="form-input"
                :placeholder="t('profile.currentPasswordPlaceholder')"
                required
              />
            </div>

            <div class="btn-group">
              <button
                type="button"
                class="secondary-btn"
                @click="emailChangeStepCodeSent = false"
              >
                {{ t('profile.back') }}
              </button>
              <button
                type="submit"
                class="primary-btn"
                :disabled="isChangingEmail"
              >
                {{ isChangingEmail ? t('profile.updatingEmail') : t('profile.submitEmailChange') }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <div v-if="activeTab === 'security'" class="tab-content">
        <div class="section-card max-w-xl">
          <h2>{{ t('profile.securityTitle') }}</h2>
          <p class="section-sub">{{ t('profile.securitySub') }}</p>

          <div v-if="!passwordStepCodeSent" class="security-step-box">
            <div class="info-alert">
              <p>{{ t('profile.registeredEmail', { email: authStore.user?.email }) }}</p>
            </div>
            <button
              class="primary-btn wide-btn"
              :disabled="isSendingPasswordCode"
              @click="handleSendPasswordCode"
            >
              {{ isSendingPasswordCode ? t('profile.sendingCode') : t('profile.sendPasswordCodeBtn') }}
            </button>
          </div>

          <form v-else class="profile-form" @submit.prevent="handleChangePassword">
            <div class="success-alert">
              <p>{{ t('profile.passwordCodeSentAlert', { email: authStore.user?.email }) }}</p>
            </div>

            <div class="form-group">
              <label for="password-code">{{ t('profile.sixDigitCode') }}</label>
              <input
                id="password-code"
                v-model="passwordForm.code"
                type="text"
                class="form-input code-input"
                placeholder="123456"
                maxlength="6"
                required
              />
            </div>

            <div class="form-group">
              <label for="new-password">{{ t('profile.newPassword') }}</label>
              <input
                id="new-password"
                v-model="passwordForm.newPassword"
                type="password"
                class="form-input"
                :placeholder="t('profile.passwordMinHint')"
                required
              />
            </div>

            <div class="form-group">
              <label for="confirm-password">{{ t('profile.confirmNewPassword') }}</label>
              <input
                id="confirm-password"
                v-model="passwordForm.confirmPassword"
                type="password"
                class="form-input"
                :placeholder="t('profile.confirmPasswordPlaceholder')"
                required
              />
            </div>

            <div class="btn-group">
              <button
                type="button"
                class="secondary-btn"
                @click="passwordStepCodeSent = false"
              >
                {{ t('profile.back') }}
              </button>
              <button
                type="submit"
                class="primary-btn"
                :disabled="isChangingPassword"
              >
                {{ isChangingPassword ? t('profile.updatingPassword') : t('profile.updatePasswordBtn') }}
              </button>
            </div>
          </form>
        </div>
      </div>

      <div v-if="activeTab === 'favorites'" class="tab-content">
        <div class="section-card">
          <h2>{{ t('profile.favoritesTitle') }}</h2>
          <p class="section-sub">{{ t('profile.favoritesSub') }}</p>

          <div v-if="isLoadingFavorites" class="loading-state">
            <LoadingSpinner size="md" />
            <p>{{ t('profile.loadingFavorites') }}</p>
          </div>

          <div v-else-if="favorites.length === 0" class="empty-state">
            <h3>{{ t('profile.noFavorites') }}</h3>
            <p>{{ t('profile.noFavoritesSub') }}</p>
          </div>

          <div v-else class="favorites-grid">
            <div
              v-for="fav in favorites"
              :key="fav.id"
              class="favorite-card clickable-card"
              @click="goToDiscoverWithRestaurant(fav.restaurantId)"
            >
              <div class="fav-card-header">
                <h3>{{ fav.restaurant?.name || t('profile.defaultRestaurantName') }}</h3>
                <span class="category-chip">{{ fav.restaurant?.category || t('profile.defaultCategory') }}</span>
              </div>
              <p v-if="fav.customNote" class="fav-note">"{{ fav.customNote }}"</p>
              <div class="fav-card-footer">
                <span class="fav-price">Ort. ₺{{ fav.restaurant?.averagePrice }}</span>
                <div class="fav-actions">
                  <button
                    class="view-discover-btn"
                    @click.stop="goToDiscoverWithRestaurant(fav.restaurantId)"
                  >
                    {{ t('profile.viewOnDiscover') }}
                  </button>
                  <button
                    class="remove-fav-btn"
                    @click.stop="handleRemoveFavorite(fav.restaurantId)"
                  >
                    {{ t('profile.removeFav') }}
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

const expenditures = ref<any[]>([])
const favorites = ref<any[]>([])
const isLoadingExpenditures = ref(false)
const isLoadingFavorites = ref(false)

const isUpdatingProfile = ref(false)
const profileForm = ref({
  name: authStore.user?.name || ''
})

const isSendingEmailCode = ref(false)
const isChangingEmail = ref(false)
const emailChangeStepCodeSent = ref(false)
const emailChangeForm = ref({
  newEmail: '',
  code: '',
  currentPassword: ''
})

const isSendingPasswordCode = ref(false)
const isChangingPassword = ref(false)
const passwordStepCodeSent = ref(false)
const passwordForm = ref({
  code: '',
  newPassword: '',
  confirmPassword: ''
})

const userInitials = computed(() => {
  const name = authStore.user?.name || 'U'
  return name
    .split(' ')
    .filter(Boolean)
    .map(n => n[0])
    .join('')
    .toUpperCase()
    .substring(0, 2) || 'U'
})

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
    const cat = item.restaurantCategory || t('profile.defaultCategory')
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

const monthlyChartData = computed(() => {
  if (expenditures.value.length === 0) return []

  const dateLocale = locale.value === 'en' ? 'en-US' : 'tr-TR'
  const monthMap: Record<string, number> = {}

  expenditures.value.forEach(item => {
    const d = new Date(item.spentAt)
    const monthKey = d.toLocaleDateString(dateLocale, { month: 'short', year: '2-digit' })
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
    const cat = item.restaurantCategory || t('profile.defaultCategory')
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

onMounted(() => {
  fetchExpenditures()
  fetchFavorites()
})

async function fetchExpenditures() {
  isLoadingExpenditures.value = true
  try {
    const res: any = await userService.getExpenditures()
    if (res?.isSuccess) {
      expenditures.value = res.data || []
    }
  } catch {
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
  } catch {
  } finally {
    isLoadingFavorites.value = false
  }
}

function goToDiscoverWithRestaurant(restaurantId: number) {
  navigateTo(`/?restaurantId=${restaurantId}`)
}

async function handleUpdateProfile() {
  if (!profileForm.value.name || profileForm.value.name.trim().length < 3) {
    toast.showWarning(t('profile.nameMinLength'))
    return
  }

  isUpdatingProfile.value = true
  try {
    const res: any = await userService.updateProfile({ name: profileForm.value.name.trim() })
    if (res?.isSuccess) {
      authStore.updateUserName(profileForm.value.name.trim())
      toast.showSuccess(t('profile.profileUpdated'))
    } else {
      toast.showError(res?.message || t('profile.profileUpdateFailed'))
    }
  } catch (err: any) {
    toast.showError(err?.data?.message || t('profile.profileUpdateFailed'))
  } finally {
    isUpdatingProfile.value = false
  }
}

async function handleSendEmailChangeCode() {
  if (!emailChangeForm.value.newEmail || !emailChangeForm.value.newEmail.includes('@')) {
    toast.showWarning(t('profile.enterValidNewEmail'))
    return
  }

  isSendingEmailCode.value = true
  try {
    const res: any = await userService.sendEmailChangeCode({ newEmail: emailChangeForm.value.newEmail.trim() })
    if (res?.isSuccess) {
      toast.showSuccess(t('profile.codeSentSuccess'))
      emailChangeStepCodeSent.value = true
    } else {
      toast.showError(res?.message || t('profile.codeSendFailed'))
    }
  } catch (err: any) {
    toast.showError(err?.data?.message || t('profile.codeSendFailed'))
  } finally {
    isSendingEmailCode.value = false
  }
}

async function handleChangeEmailSubmit() {
  if (!emailChangeForm.value.code || emailChangeForm.value.code.length < 6) {
    toast.showWarning(t('profile.enter6DigitCode'))
    return
  }

  if (!emailChangeForm.value.currentPassword) {
    toast.showWarning(t('profile.enterCurrentPassword'))
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
      toast.showSuccess(t('profile.emailChangedSuccess'))
      authStore.logout()
    } else {
      toast.showError(res?.message || t('profile.emailChangeFailed'))
    }
  } catch (err: any) {
    toast.showError(err?.data?.message || t('profile.emailChangeFailed'))
  } finally {
    isChangingEmail.value = false
  }
}

async function handleSendPasswordCode() {
  isSendingPasswordCode.value = true
  try {
    const res: any = await userService.sendPasswordCode()
    if (res?.isSuccess) {
      toast.showSuccess(t('profile.passwordCodeSent'))
      passwordStepCodeSent.value = true
    } else {
      toast.showError(res?.message || t('profile.codeSendFailed'))
    }
  } catch (err: any) {
    toast.showError(err?.data?.message || t('profile.codeSendFailed'))
  } finally {
    isSendingPasswordCode.value = false
  }
}

async function handleChangePassword() {
  if (passwordForm.value.newPassword.length < 6) {
    toast.showWarning(t('register.passwordTooShort'))
    return
  }

  if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
    toast.showWarning(t('register.passwordsDoNotMatch'))
    return
  }

  isChangingPassword.value = true
  try {
    const res: any = await userService.changePasswordWithCode({
      code: passwordForm.value.code.trim(),
      newPassword: passwordForm.value.newPassword
    })

    if (res?.isSuccess) {
      toast.showSuccess(t('profile.passwordChangedSuccess'))
      passwordStepCodeSent.value = false
      passwordForm.value = { code: '', newPassword: '', confirmPassword: '' }
    } else {
      toast.showError(res?.message || t('profile.passwordChangeFailed'))
    }
  } catch (err: any) {
    toast.showError(err?.data?.message || t('profile.passwordChangeFailed'))
  } finally {
    isChangingPassword.value = false
  }
}

async function handleRemoveFavorite(restaurantId: number) {
  try {
    const res: any = await userService.removeFavorite(restaurantId)
    if (res?.isSuccess) {
      toast.showSuccess(t('profile.favRemovedSuccess'))
      favorites.value = favorites.value.filter(f => f.restaurantId !== restaurantId)
    } else {
      toast.showError(res?.message || t('profile.favActionFailed'))
    }
  } catch {
    toast.showError(t('profile.favRemoveError'))
  }
}

function formatDate(dateStr: string) {
  if (!dateStr) return '-'
  const date = new Date(dateStr)
  const dateLocale = locale.value === 'en' ? 'en-US' : 'tr-TR'
  return date.toLocaleDateString(dateLocale, {
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

.profile-container {
  max-width: 1080px;
  margin: 0 auto;
  padding: 28px 24px 60px;
}

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
