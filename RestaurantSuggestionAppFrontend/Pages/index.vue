<template>
  <div class="sofra">
    <header>
      <div class="hbar">
        <div style="display:flex;align-items:center;gap:20px;">
          <span class="logo">Sofra</span>
          <nav><span class="on">{{ t('nav.discover') }}</span></nav>
        </div>

        <div class="header-right">
          <div class="balance">
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
      </div>

      <div class="mobile-tabs">
        <button :class="{ active: mobileTab === 'list' }" @click="mobileTab = 'list'">
          📋 Restoran Listesi ({{ filteredRestaurants.length }})
        </button>
        <button :class="{ active: mobileTab === 'map' }" @click="mobileTab = 'map'">
          🗺️ Harita Görünümü
        </button>
      </div>
    </header>

    <div class="content">
      <aside class="side-box" :class="{ 'mobile-hidden': mobileTab === 'map' }">
        <div class="kick">{{ t('sidebar.radiusPrefix') }} · {{ store.radius }} KM</div>
        <h1>{{ t('sidebar.title') }}</h1>
        <p class="note">{{ t('sidebar.subtitle') }}</p>

        <!-- Yarıçap -->
        <div class="radius-box">
          <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:6px;">
            <label for="radius-select" style="margin:0;">{{ t('sidebar.radiusLabel') }}</label>
            <LoadingSpinner v-if="store.isLoading" size="sm" />
          </div>
          <select id="radius-select" v-model="store.radius" :disabled="store.isLoading" @change="onRadiusChange">
            <option :value="1">1 KM</option>
            <option :value="2">2 KM</option>
            <option :value="5">5 KM</option>
            <option :value="10">10 KM</option>
          </select>
        </div>

        <!-- 🔍 Arama -->
        <div class="filter-box search-box">
          <div class="search-input-wrap">
            <span class="search-icon">🔍</span>
            <input
              v-model="searchQuery"
              type="text"
              :placeholder="t('filter.searchPlaceholder')"
              class="search-input"
            />
            <button v-if="searchQuery" class="search-clear" @click="searchQuery = ''">✕</button>
          </div>
        </div>

        <!-- 🏷️ Kategori Filtresi -->
        <div v-if="availableCategories.length > 1" class="filter-box">
          <label class="filter-label">{{ t('filter.category') }}</label>
          <div class="chip-scroll">
            <button
              class="chip"
              :class="{ active: selectedCategories.size === 0 }"
              @click="selectedCategories.clear(); triggerReactivity()"
            >
              {{ t('filter.allCategories') }}
            </button>
            <button
              v-for="cat in availableCategories"
              :key="cat"
              class="chip"
              :class="{ active: selectedCategories.has(cat) }"
              @click="toggleCategory(cat)"
            >
              {{ cat }}
            </button>
          </div>
        </div>

        <!-- 💰 Fiyat Aralığı Filtresi -->
        <div class="filter-box">
          <label class="filter-label">{{ t('filter.price') }}</label>
          <div class="chip-scroll">
            <button
              class="chip"
              :class="{ active: selectedPriceRange === null }"
              @click="selectedPriceRange = null"
            >
              {{ t('filter.allPrices') }}
            </button>
            <button
              v-for="range in priceRanges"
              :key="range.label"
              class="chip"
              :class="{ active: selectedPriceRange === range }"
              @click="selectedPriceRange = range"
            >
              {{ range.label }}
            </button>
          </div>
        </div>

        <!-- Aktif filtre sayacı -->
        <div v-if="activeFilterCount > 0" class="filter-status">
          <span>{{ t('filter.resultCount', { count: filteredRestaurants.length }) }}</span>
          <button class="clear-filters-btn" @click="clearAllFilters">{{ t('filter.clearFilters') }}</button>
        </div>

        <!-- Skeleton / Empty / Liste -->
        <RestaurantSkeleton v-if="store.isLoading" :count="4" />

        <div v-else-if="store.restaurants.length === 0" class="empty-state">
          <div class="empty-icon">📍</div>
          <p>{{ t('sidebar.noRestaurants') }}</p>
        </div>

        <div v-else-if="filteredRestaurants.length === 0" class="empty-state">
          <div class="empty-icon">🔎</div>
          <p>{{ t('filter.noResults') }}</p>
          <button class="clear-filters-link" @click="clearAllFilters">{{ t('filter.clearFilters') }}</button>
        </div>

        <div v-else class="list">
          <div
            v-for="r in filteredRestaurants"
            :id="'rest-card-' + r.id"
            :key="r.id"
            class="item"
            :class="{ selected: selectedRestaurantId === r.id }"
            @click="openDetail(r)"
          >
            <div class="thumb"></div>
            <div class="ibody">
              <div style="display:flex;justify-content:space-between;align-items:center;gap:6px;margin-bottom:3px;">
                <span class="iname">{{ r.name }}</span>
                <div style="display:flex;align-items:center;gap:6px;">
                  <button
                    class="fav-btn"
                    :class="{ isFav: favoriteIds.has(r.id) }"
                    :title="favoriteIds.has(r.id) ? 'Favorilerden Çıkar' : 'Favorilere Ekle'"
                    @click.stop="toggleFavorite(r.id)"
                  >
                    ★
                  </button>
                  <span v-if="r.walkingDurationInSeconds !== null" class="walk-badge">
                    🚶 {{ formatDuration(r.walkingDurationInSeconds) }}
                  </span>
                </div>
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

      <!-- Harita -->
      <div class="mapwrap" :class="{ 'mobile-hidden': mobileTab === 'list' }">
        <ClientOnly>
          <template #fallback>
            <div style="display:flex;flex-direction:column;justify-content:center;align-items:center;gap:12px;height:100%;color:#A79483;background:#17120F;">
              <LoadingSpinner size="lg" />
              <p style="font-size:14px;font-weight:600;margin:0;">{{ t('map.loading') }}</p>
            </div>
          </template>

          <RestaurantMap
            :selected-restaurant-id="selectedRestaurantId"
            @select-restaurant="onSelectRestaurantFromMap"
          />
        </ClientOnly>
      </div>
    </div>

    <!-- Detay Modal -->
    <RestaurantDetailModal
      :restaurant="detailRestaurant"
      :is-favorite="detailRestaurant ? favoriteIds.has(detailRestaurant.id) : false"
      :favorite-note="detailRestaurant ? (favoriteNotes.get(detailRestaurant.id) || '') : ''"
      @close="detailRestaurant = null"
      @toggle-favorite="handleModalFavoriteToggle"
      @save-note="handleSaveNote"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRestaurantStore } from '~/stores/restaurantStore'
import { useAuthStore } from '~/stores/authStore'
import { useRouter } from '#app'

const { locale, setLocale, t } = useI18n()
const store = useRestaurantStore()
const authStore = useAuthStore()
const router = useRouter()
const api = useApi()

const selectedRestaurantId = ref(null)
const mobileTab = ref('list')
const favoriteIds = ref(new Set())
const favoriteNotes = ref(new Map())
const detailRestaurant = ref(null)

// Filter states
const searchQuery = ref('')
const selectedCategories = ref(new Set())
const selectedPriceRange = ref(null)
const reactivityTrigger = ref(0)

const priceRanges = [
  { label: '₺0 – 100', min: 0, max: 100 },
  { label: '₺100 – 250', min: 100, max: 250 },
  { label: '₺250 – 500', min: 250, max: 500 },
  { label: '₺500+', min: 500, max: Infinity }
]

// Computed: benzersiz kategoriler
const availableCategories = computed(() => {
  const cats = new Set()
  store.restaurants.forEach(r => { if (r.category) cats.add(r.category) })
  return Array.from(cats).sort()
})

// Computed: filtrelenmiş liste
const filteredRestaurants = computed(() => {
  // reactivityTrigger dependency for Set changes
  void reactivityTrigger.value
  let list = store.restaurants

  // Arama filtresi
  if (searchQuery.value.trim()) {
    const q = searchQuery.value.trim().toLowerCase()
    list = list.filter(r => r.name.toLowerCase().includes(q))
  }

  // Kategori filtresi
  if (selectedCategories.value.size > 0) {
    list = list.filter(r => selectedCategories.value.has(r.category))
  }

  // Fiyat aralığı filtresi
  if (selectedPriceRange.value) {
    const { min, max } = selectedPriceRange.value
    list = list.filter(r => r.averagePrice >= min && r.averagePrice < max)
  }

  return list
})

// Aktif filtre sayısı
const activeFilterCount = computed(() => {
  void reactivityTrigger.value
  let count = 0
  if (searchQuery.value.trim()) count++
  if (selectedCategories.value.size > 0) count++
  if (selectedPriceRange.value) count++
  return count
})

const triggerReactivity = () => { reactivityTrigger.value++ }

const toggleCategory = (cat) => {
  if (selectedCategories.value.has(cat)) {
    selectedCategories.value.delete(cat)
  } else {
    selectedCategories.value.add(cat)
  }
  triggerReactivity()
}

const clearAllFilters = () => {
  searchQuery.value = ''
  selectedCategories.value.clear()
  selectedPriceRange.value = null
  triggerReactivity()
}

watch(mobileTab, (newTab) => {
  if (newTab === 'map') {
    setTimeout(() => {
      window.dispatchEvent(new Event('resize'))
    }, 100)
  }
})

// Favoriler
const fetchFavorites = async () => {
  try {
    const res = await api.get('/api/User/favorites')
    if (res?.isSuccess && Array.isArray(res?.data)) {
      favoriteIds.value = new Set(res.data.map(f => f.restaurantId))
      const notesMap = new Map()
      res.data.forEach(f => {
        if (f.customNote) notesMap.set(f.restaurantId, f.customNote)
      })
      favoriteNotes.value = notesMap
    }
  } catch (err) {
    console.warn('Favoriler yüklenirken hata oluştu:', err)
  }
}

const toggleFavorite = async (restaurantId) => {
  try {
    if (favoriteIds.value.has(restaurantId)) {
      await api.delete(`/api/User/favorites/${restaurantId}`)
      favoriteIds.value.delete(restaurantId)
      favoriteNotes.value.delete(restaurantId)
    } else {
      await api.post('/api/User/favorites', { restaurantId })
      favoriteIds.value.add(restaurantId)
    }
  } catch (err) {
    console.error('Favori işlem hatası:', err)
  }
}

const handleModalFavoriteToggle = async (restaurantId) => {
  await toggleFavorite(restaurantId)
}

const handleSaveNote = async ({ restaurantId, note }) => {
  try {
    await api.post('/api/User/favorites', { restaurantId, customNote: note })
    favoriteNotes.value.set(restaurantId, note)
    if (!favoriteIds.value.has(restaurantId)) {
      favoriteIds.value.add(restaurantId)
    }
    try { useToast().showSuccess(t('detail.noteSaved')) } catch (e) {}
  } catch (err) {
    console.error('Not kaydetme hatası:', err)
  }
}

// Detay modal
const openDetail = (restaurant) => {
  selectedRestaurantId.value = restaurant.id
  detailRestaurant.value = restaurant
}

const onSelectRestaurantFromMap = (id) => {
  selectedRestaurantId.value = id
  const el = document.getElementById('rest-card-' + id)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'nearest' })
  }
}

const onRadiusChange = () => {
  const savedLat = typeof window !== 'undefined' ? parseFloat(localStorage.getItem('saved_user_lat') || '41.0082') : 41.0082
  const savedLng = typeof window !== 'undefined' ? parseFloat(localStorage.getItem('saved_user_lng') || '28.9784') : 28.9784
  const lat = store.userLocation?.lat || savedLat
  const lng = store.userLocation?.lng || savedLng
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

onMounted(() => {
  fetchFavorites()
})
</script>

<style scoped>
.sofra { height: 100vh; display: flex; flex-direction: column; background: #201A16; font-family: 'Manrope', sans-serif; color: #F3E9DE; overflow: hidden; }
.sofra a { color: inherit; text-decoration: none; }

header { flex: 0 0 auto; background: rgba(23,18,15,.85); backdrop-filter: blur(12px); border-bottom: 1px solid #33291F; }
.hbar { padding: 12px 24px; display: flex; align-items: center; justify-content: space-between; gap: 16px; flex-wrap: wrap; }
.logo { font-family: 'Instrument Serif', serif; font-size: 25px; font-style: italic; }
nav { display: flex; gap: 24px; font-size: 14px; font-weight: 600; }
nav span { cursor: pointer; color: #A79483; }
nav span.on { color: #E0A458; }

.header-right { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; }

.balance { display: flex; align-items: center; gap: 10px; background: #2B231D; padding: 7px 12px; border-radius: 12px; border: 1px solid #40342A; }
.balance small { font-size: 11px; color: #A79483; font-weight: 700; letter-spacing: .06em; }
.balance b { font-size: 14px; color: #E0A458; }
.logout-btn { padding: 5px 10px; background: #40342A; color: #F3E9DE; border: none; border-radius: 8px; cursor: pointer; font-size: 11px; font-weight: 700; font-family: 'Manrope', sans-serif; }
.logout-btn:hover { background: #5A4526; }

.lang-switch { display: flex; align-items: center; gap: 4px; font-size: 11px; font-weight: 700; color: #A79483; margin-right: 4px; }
.lang-switch button { background: none; border: none; color: #A79483; cursor: pointer; font-weight: 700; font-family: 'Manrope', sans-serif; padding: 2px 4px; border-radius: 4px; transition: color .2s; }
.lang-switch button.active { color: #E0A458; background: rgba(224, 164, 88, 0.15); }

.mobile-tabs { display: none; padding: 6px 12px; background: #17120F; border-top: 1px solid #33291F; gap: 8px; }
.mobile-tabs button { flex: 1; padding: 8px; border: none; background: #2B231D; color: #A79483; font-size: 12px; font-weight: 700; border-radius: 8px; cursor: pointer; }
.mobile-tabs button.active { background: #E0A458; color: #17120F; }

.content { flex: 1; display: flex; gap: 20px; padding: 20px 24px; min-height: 0; }
.side-box { flex: 0 0 360px; min-height: 0; overflow-y: auto; background: #2B231D; border: 1px solid #40342A; border-radius: 20px; padding: 18px; }
.side-box h1 { font-family: 'Instrument Serif', serif; font-size: 24px; font-weight: 400; margin: 4px 0 2px; }
.kick { font-size: 11px; color: #E0A458; font-weight: 700; letter-spacing: .03em; }
.note { font-size: 12px; color: #8A745E; margin: 0 0 12px; }

.radius-box { margin-bottom: 10px; background: #201A16; border: 1px solid #40342A; padding: 10px; border-radius: 12px; }
.radius-box label { font-weight: 600; color: #C9B9A8; font-size: 12px; display: block; margin-bottom: 4px; }
.radius-box select { width: 100%; padding: 6px; border-radius: 8px; border: 1px solid #40342A; background-color: #201A16; font-size: 13px; color: #F3E9DE; cursor: pointer; outline: none; }

/* ─── Filtre Kutuları ─── */
.filter-box { margin-bottom: 10px; background: #201A16; border: 1px solid #40342A; padding: 10px; border-radius: 12px; }
.filter-label { font-weight: 600; color: #C9B9A8; font-size: 11px; display: block; margin-bottom: 6px; text-transform: uppercase; letter-spacing: 0.04em; }

/* Arama */
.search-box { padding: 6px 10px; }
.search-input-wrap { display: flex; align-items: center; gap: 8px; }
.search-icon { font-size: 14px; flex-shrink: 0; opacity: 0.6; }
.search-input {
  flex: 1;
  background: transparent;
  border: none;
  outline: none;
  color: #F3E9DE;
  font-family: 'Manrope', sans-serif;
  font-size: 13px;
  padding: 4px 0;
}
.search-input::placeholder { color: #5A4526; }
.search-clear {
  background: none;
  border: none;
  color: #5A4526;
  font-size: 14px;
  cursor: pointer;
  padding: 2px 4px;
  line-height: 1;
  transition: color .15s;
}
.search-clear:hover { color: #E0A458; }

/* Chip Scroll */
.chip-scroll { display: flex; gap: 6px; overflow-x: auto; padding-bottom: 2px; -ms-overflow-style: none; scrollbar-width: none; }
.chip-scroll::-webkit-scrollbar { display: none; }

.chip {
  flex-shrink: 0;
  padding: 5px 12px;
  border: 1px solid #40342A;
  background: #2B231D;
  color: #A79483;
  border-radius: 20px;
  font-size: 11px;
  font-weight: 700;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  transition: all .15s;
  white-space: nowrap;
}
.chip:hover { border-color: #E0A458; color: #E0A458; }
.chip.active { background: rgba(224, 164, 88, 0.15); border-color: #E0A458; color: #E0A458; }

/* Filtre durumu */
.filter-status {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
  padding: 6px 0;
}
.filter-status span { font-size: 11px; color: #8A745E; font-weight: 600; }
.clear-filters-btn {
  background: none;
  border: none;
  color: #E0A458;
  font-size: 11px;
  font-weight: 700;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  text-decoration: underline;
  padding: 0;
}
.clear-filters-link {
  margin-top: 10px;
  background: none;
  border: 1px solid #40342A;
  color: #E0A458;
  font-size: 12px;
  font-weight: 700;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  padding: 6px 14px;
  border-radius: 8px;
  transition: background .15s;
}
.clear-filters-link:hover { background: rgba(224, 164, 88, 0.1); }

/* ─── Liste ─── */
.list { display: flex; flex-direction: column; gap: 12px; }
.item { background: #3A2E24; border: 1px solid #5A4526; border-radius: 15px; overflow: hidden; display: flex; align-items: stretch; min-height: 74px; cursor: pointer; box-shadow: 0 6px 16px -8px rgba(0,0,0,.6); transition: transform .15s, border-color .15s, box-shadow .15s; }
.item.selected { border-color: #8FBF7E; background: #42352A; box-shadow: 0 0 14px rgba(143,191,126,0.3); }
.item:hover { transform: translateY(-2px); border-color: #E0A458; box-shadow: 0 10px 20px -8px rgba(0,0,0,.7); }
.thumb { width: 74px; min-height: 74px; background: #E0A458; flex: 0 0 74px; background-image: repeating-linear-gradient(135deg, rgba(0,0,0,.08) 0 9px, rgba(0,0,0,0) 9px 18px); }
.ibody { padding: 12px 14px; flex: 1; min-width: 0; background: #201A16; }
.iname { font-family: 'Instrument Serif', serif; font-size: 17px; }
.dist-meta { font-size: 11px; color: #8A745E; margin-top: 4px; }
.imeta { font-size: 12px; color: #A79483; }
.score-badge { display: inline-flex; align-items: center; gap: 5px; font-size: 11px; font-weight: 700; padding: 4px 10px; border-radius: 8px; margin-top: 8px; width: fit-content; }

button {
  -webkit-appearance: none !important;
  appearance: none !important;
  outline: none;
}

.fav-btn {
  -webkit-appearance: none !important;
  appearance: none !important;
  background: transparent !important;
  border: none !important;
  box-shadow: none !important;
  outline: none !important;
  color: #5A4526;
  font-size: 18px;
  cursor: pointer;
  transition: color .2s, transform .15s;
  padding: 2px 4px;
  line-height: 1;
}
.fav-btn.isFav { color: #E0A458 !important; text-shadow: 0 0 8px rgba(224,164,88,0.6); }
.fav-btn:hover { transform: scale(1.25); color: #E0A458; }

.walk-badge {
  font-size: 11px;
  color: #E0A458;
  font-weight: 700;
  background: rgba(224,164,88,0.12);
  padding: 3px 7px;
  border-radius: 6px;
  border: 1px solid rgba(224,164,88,0.25);
  display: inline-flex;
  align-items: center;
  gap: 3px;
}

.mapwrap { flex: 1; min-width: 0; position: relative; border: 1px solid #40342A; border-radius: 22px; overflow: hidden; }
.mapwrap :deep(> div) { height: 100%; }

@media (max-width: 820px) {
  .mobile-tabs { display: flex; }
  .content { padding: 12px; flex-direction: column; gap: 0; }
  .side-box { flex: 1; width: 100%; border-radius: 14px; }
  .mapwrap { flex: 1; width: 100%; min-height: 380px; border-radius: 14px; }
  .mobile-hidden { display: none !important; }
  .hbar { padding: 10px 14px; flex-direction: column; align-items: stretch; gap: 10px; }
  .header-right { justify-content: space-between; }
}

@media (max-width: 480px) {
  .balance { width: 100%; justify-content: space-between; }
  .item { min-height: 84px; }
  .iname { font-size: 15px; }
  .mobile-tabs button { font-size: 11px; padding: 10px 6px; }
  .hbar { padding: 8px 12px; }
}

.skeleton-list { display: flex; flex-direction: column; gap: 12px; }
.skeleton-item { display: flex; height: 74px; background: #201A16; border: 1px solid #33291F; border-radius: 15px; overflow: hidden; }
.skeleton-thumb { width: 74px; background: #2B231D; animation: pulse 1.5s infinite ease-in-out; }
.skeleton-body { flex: 1; padding: 12px; display: flex; flex-direction: column; gap: 8px; justify-content: center; }
.skeleton-line { background: #2B231D; border-radius: 6px; animation: pulse 1.5s infinite ease-in-out; }
.skeleton-line.title { height: 16px; width: 60%; }
.skeleton-line.meta { height: 12px; width: 85%; }

@keyframes pulse {
  0% { opacity: .4; }
  50% { opacity: .8; }
  100% { opacity: .4; }
}

.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 32px 16px;
  background: #201A16;
  border: 1px dashed #40342A;
  border-radius: 16px;
  text-align: center;
  color: #A79483;
}
.empty-icon {
  font-size: 32px;
  margin-bottom: 8px;
}
.empty-state p {
  font-size: 13px;
  line-height: 1.5;
  margin: 0;
}
</style>