<template>
  <div class="sofra">
    <header>
      <div class="hbar">
        <div style="display:flex;align-items:center;gap:32px;">
          <span class="logo">Sofra</span>
          <nav><span class="on">{{ t('nav.discover') }}</span></nav>
        </div>
        <div class="balance">
          <!-- TR / EN DİL DEĞİŞTİRME BUTONU -->
          <div class="lang-switch">
            <button :class="{ active: locale === 'tr' }" @click="setLocale('tr')">TR</button>
            <span>|</span>
            <button :class="{ active: locale === 'en' }" @click="setLocale('en')">EN</button>
          </div>
          <small>{{ t('nav.hello') }}</small>
          <b>{{ authStore.user?.name }}</b>
          <button class="logout-btn" @click="handleLogout">{{ t('nav.logout') }}</button>
        </div>
      </div>
    </header>

    <div class="content">
      <!-- Sol Panel: Restoran Listesi -->
      <aside class="side-box">
        <div class="kick">{{ t('sidebar.radiusPrefix') }} · {{ store.radius }} KM</div>
        <h1>{{ t('sidebar.title') }}</h1>
        <p class="note">{{ t('sidebar.subtitle') }}</p>

        <div class="radius-box">
          <label for="radius-select">{{ t('sidebar.radiusLabel') }}</label>
          <select id="radius-select" v-model="store.radius" @change="onRadiusChange">
            <option :value="1">1 KM</option>
            <option :value="2">2 KM</option>
            <option :value="5">5 KM</option>
            <option :value="10">10 KM</option>
          </select>
        </div>

        <p v-if="store.isLoading" class="note">{{ t('sidebar.loading') }}</p>

        <div v-else class="list">
          <div
            v-for="r in store.restaurants"
            :key="r.id"
            class="item"
          >
            <div class="thumb"></div>
            <div class="ibody">
              <div style="display:flex;justify-content:space-between;align-items:center;gap:6px;margin-bottom:3px;">
                <span class="iname">{{ r.name }}</span>
                <span v-if="r.walkingDurationInSeconds !== null" class="walk-badge">
                  🚶 {{ formatDuration(r.walkingDurationInSeconds) }}
                </span>
              </div>
              <div class="imeta">{{ r.category }} · ₺{{ r.averagePrice }}</div>
              <div v-if="r.walkingDistanceInMeters !== null" class="dist-meta">
                {{ t('sidebar.distance') }}: {{ formatDistance(r.walkingDistanceInMeters) }}
              </div>
              <div 
                v-if="r.recommendationScore !== null" 
                class="score-badge" 
                :style="getScoreStyle(r.recommendationScore)"
              >
                🎯 {{ t('badge.recommendation', { score: Math.round(r.recommendationScore) }) }}
              </div>
            </div>
          </div>
        </div>
      </aside>

      <!-- Sağ Panel: Harita -->
      <div class="mapwrap">
        <ClientOnly>
          <template #fallback>
            <div style="display:flex;justify-content:center;align-items:center;height:100%;color:#A79483;">
              <p>{{ t('map.loading') }}</p>
            </div>
          </template>

          <RestaurantMap />
        </ClientOnly>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRestaurantStore } from '~/Stores/restaurantStore'
import { useAuthStore } from '~/Stores/authStore'
import { useRouter } from '#app'

const { locale, setLocale, t } = useI18n()
const store = useRestaurantStore()
const authStore = useAuthStore()
const router = useRouter()

const onRadiusChange = () => {
  const lat = store.userLocation?.lat || 40.9901
  const lng = store.userLocation?.lng || 29.0256
  store.fetchNearbyRestaurants(lat, lng, store.radius)
}

const formatDistance = (meters) => {
  if (meters === null || meters === undefined) return '';
  if (meters < 1000) return `${Math.round(meters)} m`;
  return `${(meters / 1000).toFixed(1)} km`;
}

const formatDuration = (seconds) => {
  if (seconds === null || seconds === undefined) return '';
  const minutes = Math.round(seconds / 60);
  return `${minutes} dk`;
}

const handleLogout = () => {
  authStore.logout()
  router.push('/login')
}

const getScoreStyle = (score) => {
  if (score >= 80) {
    return 'background: rgba(40, 167, 69, 0.15); color: #8FBF7E; border: 1px solid rgba(40, 167, 69, 0.3);'
  }
  if (score >= 50) {
    return 'background: rgba(224, 164, 88, 0.12); color: #E0A458; border: 1px solid rgba(224, 164, 88, 0.25);'
  }
  return 'background: rgba(220, 53, 69, 0.15); color: #E08E58; border: 1px solid rgba(220, 53, 69, 0.3);'
}
</script>

<style scoped>
.sofra { height: 100vh; display: flex; flex-direction: column; background: #201A16; font-family: 'Manrope', sans-serif; color: #F3E9DE; overflow: hidden; }
.sofra a { color: inherit; text-decoration: none; }

header { flex: 0 0 auto; background: rgba(23,18,15,.85); backdrop-filter: blur(12px); border-bottom: 1px solid #33291F; }
.hbar { padding: 16px 28px; display: flex; align-items: center; justify-content: space-between; gap: 20px; }
.logo { font-family: 'Instrument Serif', serif; font-size: 25px; font-style: italic; }
nav { display: flex; gap: 24px; font-size: 14px; font-weight: 600; }
nav span { cursor: pointer; color: #A79483; }
nav span.on { color: #E0A458; }
.balance { display: flex; align-items: center; gap: 12px; background: #2B231D; padding: 9px 15px; border-radius: 12px; border: 1px solid #40342A; }
.balance small { font-size: 11px; color: #A79483; font-weight: 700; letter-spacing: .06em; }
.balance b { font-size: 15px; color: #E0A458; }
.logout-btn { padding: 6px 12px; background: #40342A; color: #F3E9DE; border: none; border-radius: 8px; cursor: pointer; font-size: 12px; font-weight: 700; font-family: 'Manrope', sans-serif; }
.logout-btn:hover { background: #5A4526; }

.lang-switch { display: flex; align-items: center; gap: 4px; font-size: 12px; font-weight: 700; color: #A79483; margin-right: 6px; }
.lang-switch button { background: none; border: none; color: #A79483; cursor: pointer; font-weight: 700; font-family: 'Manrope', sans-serif; padding: 2px 4px; border-radius: 4px; transition: color .2s; }
.lang-switch button.active { color: #E0A458; background: rgba(224, 164, 88, 0.15); }
.lang-switch button:hover { color: #F3E9DE; }

.content { flex: 1; display: flex; gap: 20px; padding: 20px 28px; min-height: 0; }
.side-box { flex: 0 0 360px; min-height: 0; overflow-y: auto; background: #2B231D; border: 1px solid #40342A; border-radius: 20px; padding: 20px; }
.side-box h1 { font-family: 'Instrument Serif', serif; font-size: 26px; font-weight: 400; margin: 6px 0 4px; }
.kick { font-size: 12px; color: #E0A458; font-weight: 700; letter-spacing: .03em; }
.note { font-size: 12.5px; color: #8A745E; margin: 0 0 14px; }

.radius-box { margin-bottom: 16px; background: #201A16; border: 1px solid #40342A; padding: 12px; border-radius: 12px; }
.radius-box label { font-weight: 600; color: #C9B9A8; font-size: 13px; display: block; margin-bottom: 6px; }
.radius-box select { width: 100%; padding: 8px; border-radius: 8px; border: 1px solid #40342A; background-color: #201A16; font-size: 14px; color: #F3E9DE; cursor: pointer; outline: none; }

.list { display: flex; flex-direction: column; gap: 14px; }
.item { background: #3A2E24; border: 1px solid #5A4526; border-radius: 15px; overflow: hidden; display: flex; align-items: stretch; min-height: 74px; margin-bottom: 14px; box-shadow: 0 6px 16px -8px rgba(0,0,0,.6); transition: transform .15s, border-color .15s, box-shadow .15s; }
.item:last-child { margin-bottom: 0; }
.item:hover { transform: translateY(-2px); border-color: #E0A458; box-shadow: 0 10px 20px -8px rgba(0,0,0,.7); }
.thumb { width: 74px; min-height: 74px; background: #E0A458; flex: 0 0 74px; background-image: repeating-linear-gradient(135deg, rgba(0,0,0,.08) 0 9px, rgba(0,0,0,0) 9px 18px); }
.ibody { padding: 12px 14px; flex: 1; min-width: 0; background: #201A16; }
.iname { font-family: 'Instrument Serif', serif; font-size: 17px; }
.dist-meta { font-size: 11px; color: #8A745E; margin-top: 4px; }
.imeta { font-size: 12px; color: #A79483; }
.score-badge { display: inline-flex; align-items: center; gap: 5px; font-size: 11px; font-weight: 700; padding: 4px 10px; border-radius: 8px; margin-top: 8px; width: fit-content; }
.mapwrap { flex: 1; min-width: 0; position: relative; border: 1px solid #40342A; border-radius: 22px; overflow: hidden; }
.mapwrap :deep(> div) { height: 100%; }
</style>