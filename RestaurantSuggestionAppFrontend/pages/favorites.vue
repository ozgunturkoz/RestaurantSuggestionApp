<template>
  <div class="favorites-page">
    <header>
      <div class="hbar">
        <div style="display:flex;align-items:center;gap:20px;">
          <span class="logo">Sofra</span>
          <nav>
            <NuxtLink to="/" class="nav-link">{{ t('nav.discover') }}</NuxtLink>
            <NuxtLink to="/favorites" class="nav-link on">{{ t('nav.favorites') }}</NuxtLink>
            <NuxtLink to="/profile" class="nav-link">{{ t('profile.myProfile') }}</NuxtLink>
          </nav>
        </div>

        <div class="header-right">
          <div class="user-info-box">
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
    </header>

    <main class="page-body">
      <div class="page-title-row">
        <div>
          <h1 class="page-title">{{ t('favorites.title') }}</h1>
          <p class="page-subtitle">{{ t('favorites.subtitle') }}</p>
        </div>
        <NuxtLink to="/" class="explore-link-btn">
          <span>🧭</span> {{ t('favorites.exploreRestaurants') }}
        </NuxtLink>
      </div>

      <!-- Quick Stats -->
      <div v-if="!isLoading && favorites.length > 0" class="stats-row">
        <div class="stat-card">
          <span class="stat-label">{{ t('favorites.totalCount') }}</span>
          <strong class="stat-val">{{ favorites.length }}</strong>
        </div>
        <div class="stat-card">
          <span class="stat-label">{{ t('favorites.avgPrice') }}</span>
          <strong class="stat-val">₺{{ overallAvgPrice }}</strong>
        </div>
        <div class="stat-card">
          <span class="stat-label">{{ t('favorites.categories') }}</span>
          <strong class="stat-val">{{ uniqueCategories.length }}</strong>
        </div>
      </div>

      <!-- Search & Filters -->
      <div v-if="!isLoading && favorites.length > 0" class="filters-bar">
        <div class="search-input-wrap">
          <span class="search-icon">🔍</span>
          <input
            v-model="searchQuery"
            type="text"
            :placeholder="t('favorites.searchPlaceholder')"
            class="search-input"
          />
          <button v-if="searchQuery" class="search-clear" @click="searchQuery = ''">✕</button>
        </div>

        <div v-if="uniqueCategories.length > 1" class="category-chips">
          <button
            class="cat-chip"
            :class="{ active: selectedCategory === null }"
            @click="selectedCategory = null"
          >
            {{ t('favorites.allCategories') }}
          </button>
          <button
            v-for="cat in uniqueCategories"
            :key="cat"
            class="cat-chip"
            :class="{ active: selectedCategory === cat }"
            @click="selectedCategory = cat"
          >
            {{ cat }}
          </button>
        </div>
      </div>

      <!-- Loading State -->
      <div v-if="isLoading" class="loading-state">
        <LoadingSpinner size="lg" />
        <p>{{ t('common.loading') }}</p>
      </div>

      <!-- Empty State: No Favorites -->
      <div v-else-if="favorites.length === 0" class="empty-state">
        <div class="empty-icon">❤️</div>
        <h2>{{ t('favorites.noFavorites') }}</h2>
        <p>{{ t('favorites.noFavoritesSub') }}</p>
        <NuxtLink to="/" class="primary-btn">
          {{ t('favorites.exploreRestaurants') }}
        </NuxtLink>
      </div>

      <!-- Empty State: Search Results 0 -->
      <div v-else-if="filteredFavorites.length === 0" class="empty-state">
        <div class="empty-icon">🔍</div>
        <h2>{{ t('favorites.noSearchResults') }}</h2>
        <p>{{ t('favorites.noSearchResultsSub') }}</p>
        <button class="secondary-btn" @click="searchQuery = ''; selectedCategory = null">
          {{ t('favorites.allCategories') }}
        </button>
      </div>

      <!-- Favorites Grid -->
      <div v-else class="favorites-grid">
        <div
          v-for="fav in filteredFavorites"
          :key="fav.id"
          class="fav-card"
        >
          <div class="fav-card-top">
            <div class="fav-card-title-group">
              <h3>{{ fav.restaurant?.name || t('profile.defaultRestaurantName') }}</h3>
              <div class="badge-row">
                <span class="category-chip">{{ fav.restaurant?.category || t('profile.defaultCategory') }}</span>
              </div>
            </div>
            <div class="fav-price-badge">
              <span>Ort.</span>
              <strong>₺{{ fav.restaurant?.averagePrice }}</strong>
            </div>
          </div>

          <!-- Custom Note Section -->
          <div class="note-section">
            <!-- Edit Note Mode -->
            <div v-if="editingNoteId === fav.restaurantId" class="note-edit-box">
              <textarea
                v-model="tempNote"
                class="note-textarea"
                rows="2"
                :placeholder="t('favorites.notePlaceholder')"
              ></textarea>
              <div class="note-edit-actions">
                <button
                  class="save-note-btn"
                  :disabled="isSavingNote"
                  @click="saveNote(fav.restaurantId)"
                >
                  {{ isSavingNote ? t('common.loading') : t('favorites.saveNote') }}
                </button>
                <button
                  class="cancel-note-btn"
                  :disabled="isSavingNote"
                  @click="cancelEditNote"
                >
                  {{ t('favorites.cancel') }}
                </button>
              </div>
            </div>

            <!-- View Note Mode -->
            <div v-else class="note-display">
              <p v-if="fav.customNote" class="existing-note">
                <span class="quote-mark">“</span>{{ fav.customNote }}<span class="quote-mark">”</span>
              </p>
              <div class="note-btn-row">
                <button
                  class="edit-note-btn"
                  @click="startEditNote(fav.restaurantId, fav.customNote)"
                >
                  {{ fav.customNote ? t('favorites.editNote') : t('favorites.addNote') }}
                </button>
              </div>
            </div>
          </div>

          <!-- Card Footer Actions -->
          <div class="fav-card-footer">
            <button
              class="discover-btn"
              @click="goToDiscoverWithRestaurant(fav.restaurantId)"
            >
              <span>🧭</span> {{ t('favorites.viewOnDiscover') }}
            </button>
            <button
              class="remove-btn"
              @click="handleRemoveFavorite(fav.restaurantId)"
            >
              <span>🗑️</span> {{ t('favorites.remove') }}
            </button>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useUserService } from '~/composables/useUserService'
import { useAuthStore } from '~/stores/authStore'
import { useI18n } from '~/composables/useI18n'
import { useToast } from '~/composables/useToast'

const { t, locale, setLocale } = useI18n()
const authStore = useAuthStore()
const userService = useUserService()
const toast = useToast()

const favorites = ref<any[]>([])
const isLoading = ref(true)
const searchQuery = ref('')
const selectedCategory = ref<string | null>(null)

// Note editing state
const editingNoteId = ref<number | null>(null)
const tempNote = ref('')
const isSavingNote = ref(false)

const uniqueCategories = computed(() => {
  const cats = new Set<string>()
  favorites.value.forEach(f => {
    if (f.restaurant?.category) {
      cats.add(f.restaurant.category)
    }
  })
  return Array.from(cats)
})

const overallAvgPrice = computed(() => {
  if (favorites.value.length === 0) return '0'
  const total = favorites.value.reduce((acc, f) => acc + (f.restaurant?.averagePrice || 0), 0)
  return Math.round(total / favorites.value.length).toString()
})

const filteredFavorites = computed(() => {
  let list = favorites.value

  if (selectedCategory.value) {
    list = list.filter(f => f.restaurant?.category === selectedCategory.value)
  }

  if (searchQuery.value.trim()) {
    const q = searchQuery.value.trim().toLowerCase()
    list = list.filter(f => {
      const name = (f.restaurant?.name || '').toLowerCase()
      const cat = (f.restaurant?.category || '').toLowerCase()
      const note = (f.customNote || '').toLowerCase()
      return name.includes(q) || cat.includes(q) || note.includes(q)
    })
  }

  return list
})

onMounted(() => {
  loadFavorites()
})

async function loadFavorites() {
  isLoading.value = true
  try {
    const res: any = await userService.getFavorites()
    favorites.value = res.data || []
  } catch (err) {
    toast.showError(t('common.error'))
  } finally {
    isLoading.value = false
  }
}

function startEditNote(restaurantId: number, currentNote: string | null) {
  editingNoteId.value = restaurantId
  tempNote.value = currentNote || ''
}

function cancelEditNote() {
  editingNoteId.value = null
  tempNote.value = ''
}

async function saveNote(restaurantId: number) {
  isSavingNote.value = true
  try {
    const res: any = await userService.addFavorite(restaurantId, tempNote.value.trim())
    if (res?.isSuccess) {
      toast.showSuccess(t('favorites.favNoteSaved'))
      const fav = favorites.value.find(f => f.restaurantId === restaurantId)
      if (fav) {
        fav.customNote = tempNote.value.trim()
      }
      cancelEditNote()
    } else {
      toast.showError(res?.message || t('common.error'))
    }
  } catch {
    toast.showError(t('common.error'))
  } finally {
    isSavingNote.value = false
  }
}

async function handleRemoveFavorite(restaurantId: number) {
  try {
    const res: any = await userService.removeFavorite(restaurantId)
    if (res?.isSuccess) {
      toast.showSuccess(t('favorites.favRemovedSuccess'))
      favorites.value = favorites.value.filter(f => f.restaurantId !== restaurantId)
    } else {
      toast.showError(res?.message || t('common.error'))
    }
  } catch {
    toast.showError(t('common.error'))
  }
}

function goToDiscoverWithRestaurant(restaurantId: number) {
  navigateTo(`/?restaurantId=${restaurantId}`)
}

function handleLogout() {
  authStore.logout()
}
</script>

<style scoped>
.favorites-page {
  min-height: 100vh;
  background: #17120F;
  font-family: 'Manrope', sans-serif;
  color: #F3E9DE;
  display: flex;
  flex-direction: column;
}

header {
  flex: 0 0 auto;
  background: rgba(23, 18, 15, 0.9);
  backdrop-filter: blur(12px);
  border-bottom: 1px solid #33291F;
}

.hbar {
  padding: 12px 28px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  flex-wrap: wrap;
}

.logo {
  font-family: 'Instrument Serif', serif;
  font-size: 26px;
  font-style: italic;
  color: #F3E9DE;
}

nav {
  display: flex;
  gap: 24px;
  font-size: 14px;
  font-weight: 600;
  align-items: center;
}

nav a {
  cursor: pointer;
  color: #A79483;
  text-decoration: none;
  transition: color 0.2s;
}

nav a.on, nav a:hover {
  color: #E0A458;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.user-info-box {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 13px;
}

.lang-switch {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: #A79483;
  margin-right: 8px;
}

.lang-switch button {
  background: none;
  border: none;
  color: #A79483;
  cursor: pointer;
  padding: 2px 4px;
  font-size: 12px;
  font-weight: 600;
  border-radius: 3px;
}

.lang-switch button.active {
  color: #E0A458;
  background: rgba(224, 164, 88, 0.15);
}

.logout-btn {
  background: rgba(224, 164, 88, 0.12);
  border: 1px solid rgba(224, 164, 88, 0.25);
  color: #E0A458;
  padding: 6px 14px;
  border-radius: 6px;
  font-size: 12px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.logout-btn:hover {
  background: #E0A458;
  color: #17120F;
}

.page-body {
  flex: 1;
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
  padding: 32px 24px 60px;
}

.page-title-row {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  gap: 16px;
  margin-bottom: 28px;
  flex-wrap: wrap;
}

.page-title {
  font-family: 'Instrument Serif', serif;
  font-size: 38px;
  font-weight: 400;
  margin: 0 0 6px 0;
  color: #F3E9DE;
}

.page-subtitle {
  font-size: 14px;
  color: #A79483;
  margin: 0;
}

.explore-link-btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(224, 164, 88, 0.12);
  border: 1px solid rgba(224, 164, 88, 0.3);
  color: #E0A458;
  padding: 9px 18px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s;
}

.explore-link-btn:hover {
  background: #E0A458;
  color: #17120F;
}

/* Stats Row */
.stats-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
  margin-bottom: 28px;
}

.stat-card {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 12px;
  padding: 16px 20px;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.stat-label {
  font-size: 12px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  color: #A79483;
}

.stat-val {
  font-size: 24px;
  color: #E0A458;
  font-family: 'Instrument Serif', serif;
}

/* Filters Bar */
.filters-bar {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 12px;
  padding: 16px 20px;
  margin-bottom: 28px;
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.search-input-wrap {
  position: relative;
  width: 100%;
}

.search-icon {
  position: absolute;
  left: 14px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 14px;
  color: #A79483;
}

.search-input {
  width: 100%;
  background: #17120F;
  border: 1px solid #3A2F24;
  border-radius: 8px;
  padding: 10px 38px 10px 38px;
  color: #F3E9DE;
  font-size: 14px;
  box-sizing: border-box;
}

.search-input:focus {
  outline: none;
  border-color: #E0A458;
}

.search-clear {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: #A79483;
  cursor: pointer;
  font-size: 12px;
}

.category-chips {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.cat-chip {
  background: #17120F;
  border: 1px solid #33291F;
  color: #A79483;
  padding: 6px 14px;
  border-radius: 20px;
  font-size: 13px;
  cursor: pointer;
  transition: all 0.2s;
}

.cat-chip:hover {
  border-color: #E0A458;
  color: #F3E9DE;
}

.cat-chip.active {
  background: #E0A458;
  border-color: #E0A458;
  color: #17120F;
  font-weight: 600;
}

/* Grid & Cards */
.favorites-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 20px;
}

.fav-card {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 14px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 16px;
  transition: transform 0.2s, border-color 0.2s;
}

.fav-card:hover {
  transform: translateY(-2px);
  border-color: rgba(224, 164, 88, 0.4);
}

.fav-card-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 12px;
}

.fav-card-title-group h3 {
  font-family: 'Instrument Serif', serif;
  font-size: 22px;
  font-weight: 400;
  margin: 0 0 6px 0;
  color: #F3E9DE;
}

.badge-row {
  display: flex;
  gap: 6px;
  flex-wrap: wrap;
}

.category-chip {
  background: rgba(224, 164, 88, 0.12);
  color: #E0A458;
  padding: 3px 10px;
  border-radius: 12px;
  font-size: 11px;
  font-weight: 600;
}

.fav-price-badge {
  text-align: right;
  flex-shrink: 0;
}

.fav-price-badge span {
  display: block;
  font-size: 10px;
  color: #A79483;
  text-transform: uppercase;
}

.fav-price-badge strong {
  font-size: 16px;
  color: #F3E9DE;
}

/* Note Section */
.note-section {
  background: #17120F;
  border: 1px dashed #3A2F24;
  border-radius: 8px;
  padding: 12px;
  font-size: 13px;
}

.existing-note {
  margin: 0 0 8px 0;
  color: #D6C7B8;
  font-style: italic;
  line-height: 1.4;
}

.quote-mark {
  color: #E0A458;
  font-size: 15px;
}

.note-btn-row {
  display: flex;
  justify-content: flex-end;
}

.edit-note-btn {
  background: none;
  border: none;
  color: #E0A458;
  font-size: 12px;
  cursor: pointer;
  padding: 0;
  font-weight: 600;
}

.edit-note-btn:hover {
  text-decoration: underline;
}

.note-edit-box {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.note-textarea {
  width: 100%;
  background: #201A16;
  border: 1px solid #4D3D2F;
  border-radius: 6px;
  padding: 8px;
  color: #F3E9DE;
  font-size: 13px;
  box-sizing: border-box;
  resize: vertical;
}

.note-textarea:focus {
  outline: none;
  border-color: #E0A458;
}

.note-edit-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.save-note-btn {
  background: #E0A458;
  color: #17120F;
  border: none;
  padding: 4px 12px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
}

.cancel-note-btn {
  background: none;
  border: 1px solid #4D3D2F;
  color: #A79483;
  padding: 4px 10px;
  border-radius: 4px;
  font-size: 12px;
  cursor: pointer;
}

/* Footer Actions */
.fav-card-footer {
  display: flex;
  gap: 10px;
  border-top: 1px solid #2B231B;
  padding-top: 14px;
}

.discover-btn {
  flex: 1;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  background: rgba(224, 164, 88, 0.15);
  border: 1px solid rgba(224, 164, 88, 0.35);
  color: #E0A458;
  padding: 8px 12px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.discover-btn:hover {
  background: #E0A458;
  color: #17120F;
}

.remove-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
  background: none;
  border: 1px solid #3A2F24;
  color: #A79483;
  padding: 8px 12px;
  border-radius: 8px;
  font-size: 13px;
  cursor: pointer;
  transition: all 0.2s;
}

.remove-btn:hover {
  border-color: #E08E58;
  color: #E08E58;
}

/* Loading & Empty States */
.loading-state, .empty-state {
  text-align: center;
  padding: 60px 20px;
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 16px;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 12px;
}

.empty-state h2 {
  font-family: 'Instrument Serif', serif;
  font-size: 28px;
  margin: 0 0 8px 0;
}

.empty-state p {
  color: #A79483;
  font-size: 14px;
  max-width: 440px;
  margin: 0 auto 20px auto;
}

.primary-btn {
  display: inline-block;
  background: #E0A458;
  color: #17120F;
  padding: 10px 24px;
  border-radius: 8px;
  font-weight: 600;
  font-size: 14px;
  text-decoration: none;
  border: none;
  cursor: pointer;
}

.secondary-btn {
  background: none;
  border: 1px solid #3A2F24;
  color: #F3E9DE;
  padding: 8px 18px;
  border-radius: 8px;
  font-size: 13px;
  cursor: pointer;
}

@media (max-width: 768px) {
  .page-title-row {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
