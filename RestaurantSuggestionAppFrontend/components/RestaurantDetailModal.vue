<template>

  <Teleport to="body">
    <Transition name="modal">
      <div v-if="restaurant" class="modal-overlay" @click.self="$emit('close')">
        <div class="modal-card">

          <div class="modal-header">
            <div class="modal-header-info">
              <div style="display:flex; gap:8px; align-items:center; flex-wrap:wrap; margin-bottom:4px;">
                <span class="modal-category-chip">{{ restaurant.category }}</span>
                <span v-if="restaurant.isOutsideRadius" class="outside-radius-chip">
                  ❤️ {{ t('favorites.outsideRadiusBadge') }}
                </span>
              </div>
              <h2 class="modal-title">{{ restaurant.name }}</h2>
            </div>
            <button class="modal-close" @click="$emit('close')" :title="t('detail.close')">✕</button>
          </div>

          <div class="modal-body">

            <div class="stats-grid">
              <div class="stat-card">
                <div class="stat-info">
                  <small>{{ t('detail.price') }}</small>
                  <b>₺{{ restaurant.averagePrice }}</b>
                </div>
              </div>

              <div v-if="restaurant.walkingDistanceInMeters != null" class="stat-card">
                <div class="stat-info">
                  <small>{{ t('detail.distance') }}</small>
                  <b>{{ formatDistance(restaurant.walkingDistanceInMeters) }}</b>
                </div>
              </div>

              <div v-if="restaurant.walkingDurationInSeconds != null" class="stat-card">
                <div class="stat-info">
                  <small>{{ t('detail.duration') }}</small>
                  <b>{{ formatDuration(restaurant.walkingDurationInSeconds) }}</b>
                </div>
              </div>

              <div v-if="restaurant.recommendationScore != null" class="stat-card accent">
                <div class="stat-info">
                  <small>{{ t('detail.score') }}</small>
                  <b>%{{ Math.round(restaurant.recommendationScore) }}</b>
                </div>
              </div>
            </div>

            <div v-if="restaurant.locationScore != null || restaurant.budgetScore != null" class="score-breakdown">
              <div class="score-row" v-if="restaurant.locationScore != null">
                <span class="score-label">{{ t('detail.locationScore') }}</span>
                <div class="score-bar-wrap">
                  <div class="score-bar" :style="{ width: Math.round(restaurant.locationScore) + '%' }"></div>
                </div>
                <span class="score-value">%{{ Math.round(restaurant.locationScore) }}</span>
              </div>
              <div class="score-row" v-if="restaurant.budgetScore != null">
                <span class="score-label">{{ t('detail.budgetScore') }}</span>
                <div class="score-bar-wrap">
                  <div class="score-bar budget" :style="{ width: Math.round(restaurant.budgetScore) + '%' }"></div>
                </div>
                <span class="score-value">%{{ Math.round(restaurant.budgetScore) }}</span>
              </div>
            </div>

            <div v-if="restaurant.latitude && restaurant.longitude" class="mini-map-section">
              <ClientOnly>
                <div ref="miniMapEl" class="mini-map"></div>
              </ClientOnly>
            </div>

            <div class="favorite-section">
              <div class="fav-toggle-row">
                <button
                  class="fav-action-btn"
                  :class="{ active: isFavorite }"
                  @click="handleFavoriteToggle"
                >
                  <span class="fav-star">★</span>
                  {{ isFavorite ? t('detail.removeFavorite') : t('detail.addFavorite') }}
                </button>
              </div>

              <div v-if="isFavorite" class="fav-note-area">
                <label>{{ t('detail.favoriteNote') }}</label>
                <textarea
                  v-model="noteText"
                  :placeholder="t('detail.favoriteNotePlaceholder')"
                  rows="3"
                  maxlength="500"
                ></textarea>
                <div class="note-actions">
                  <span class="char-count">{{ noteText.length }}/500</span>
                  <button class="save-note-btn" @click="handleSaveNote" :disabled="savingNote">
                    {{ savingNote ? '...' : t('detail.saveNote') }}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { ref, watch, nextTick, onBeforeUnmount } from 'vue'

const props = defineProps({
  restaurant: { type: Object, default: null },
  isFavorite: { type: Boolean, default: false },
  favoriteNote: { type: String, default: '' }
})

const emit = defineEmits(['close', 'toggle-favorite', 'save-note'])

const { t, locale } = useI18n()

const noteText = ref('')
const savingNote = ref(false)
const miniMapEl = ref(null)
let miniMapInstance = null

const destroyMiniMap = () => {
  if (miniMapInstance) {
    miniMapInstance.remove()
    miniMapInstance = null
  }
}

const initMiniMap = async () => {
  await nextTick()
  if (!miniMapEl.value || !props.restaurant) return
  destroyMiniMap()

  try {
    const L = (await import('leaflet')).default
    await import('leaflet/dist/leaflet.css')

    const { latitude, longitude } = props.restaurant
    miniMapInstance = L.map(miniMapEl.value, {
      center: [latitude, longitude],
      zoom: 15,
      zoomControl: false,
      dragging: false,
      scrollWheelZoom: false,
      doubleClickZoom: false,
      attributionControl: false
    })

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      className: 'map-tiles-dark'
    }).addTo(miniMapInstance)

    const icon = L.divIcon({
      html: `<div style="
        background-color: #E0A458;
        width: 18px; height: 18px;
        border-radius: 50% 50% 50% 0;
        transform: rotate(-45deg);
        border: 2px solid white;
        box-shadow: 0 2px 6px rgba(0,0,0,0.4);
      "></div>`,
      className: 'custom-marker',
      iconSize: [18, 18],
      iconAnchor: [9, 18]
    })

    L.marker([latitude, longitude], { icon }).addTo(miniMapInstance)

    setTimeout(() => {
      if (miniMapInstance) miniMapInstance.invalidateSize()
    }, 200)
  } catch {
  }
}

watch(() => props.restaurant, (newVal) => {
  if (newVal) {
    noteText.value = props.favoriteNote || ''
    nextTick(() => {
      initMiniMap()
    })
    if (typeof document !== 'undefined') {
      document.body.style.overflow = 'hidden'
    }
  } else {
    destroyMiniMap()
    if (typeof document !== 'undefined') {
      document.body.style.overflow = ''
    }
  }
}, { immediate: true })

watch(() => props.favoriteNote, (newNote) => {
  noteText.value = newNote || ''
})

const handleFavoriteToggle = () => {
  emit('toggle-favorite', props.restaurant.id)
}

const handleSaveNote = async () => {
  savingNote.value = true
  emit('save-note', { restaurantId: props.restaurant.id, note: noteText.value })
  setTimeout(() => { savingNote.value = false }, 600)
}

const formatDistance = (meters) => {
  if (meters == null) return ''
  if (meters < 1000) return `${Math.round(meters)} m`
  return `${(meters / 1000).toFixed(1)} km`
}

const formatDuration = (seconds) => {
  if (seconds == null) return ''
  const unit = locale.value === 'en' ? 'min' : 'dk'
  return `${Math.round(seconds / 60)} ${unit}`
}

onBeforeUnmount(() => {
  destroyMiniMap()
  if (typeof document !== 'undefined') {
    document.body.style.overflow = ''
  }
})

</script>

<style scoped>
.modal-enter-active { transition: opacity .25s ease; }
.modal-enter-active .modal-card { transition: transform .3s cubic-bezier(.34,1.56,.64,1), opacity .25s ease; }
.modal-leave-active { transition: opacity .2s ease; }
.modal-leave-active .modal-card { transition: transform .2s ease, opacity .2s ease; }
.modal-enter-from { opacity: 0; }
.modal-enter-from .modal-card { transform: translateY(40px) scale(0.95); opacity: 0; }
.modal-leave-to { opacity: 0; }
.modal-leave-to .modal-card { transform: translateY(20px) scale(0.97); opacity: 0; }

.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.65);
  backdrop-filter: blur(6px);
  padding: 20px;
}

.modal-card {
  background: #2B231D;
  border: 1px solid #40342A;
  border-radius: 24px;
  width: 100%;
  max-width: 480px;
  max-height: 85vh;
  overflow-y: auto;
  box-shadow: 0 24px 64px rgba(0, 0, 0, 0.6);
  font-family: 'Manrope', sans-serif;
  color: #F3E9DE;
}

.modal-card::-webkit-scrollbar { width: 6px; }
.modal-card::-webkit-scrollbar-track { background: transparent; }
.modal-card::-webkit-scrollbar-thumb { background: #40342A; border-radius: 3px; }

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  padding: 24px 24px 16px;
  border-bottom: 1px solid #33291F;
}

.modal-header-info { flex: 1; min-width: 0; }

.modal-category-chip {
  display: inline-block;
  font-size: 10px;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: #E0A458;
  background: rgba(224, 164, 88, 0.12);
  border: 1px solid rgba(224, 164, 88, 0.25);
  padding: 3px 10px;
  border-radius: 6px;
  margin-bottom: 8px;
}

.outside-radius-chip {
  font-size: 11px;
  font-weight: 700;
  color: #E0A458;
  background: rgba(224, 164, 88, 0.2);
  border: 1px solid rgba(224, 164, 88, 0.4);
  padding: 3px 10px;
  border-radius: 6px;
  margin-bottom: 8px;
}

.modal-title {
  font-family: 'Instrument Serif', serif;
  font-size: 26px;
  font-weight: 400;
  margin: 0;
  line-height: 1.2;
}

.modal-close {
  background: #201A16;
  border: 1px solid #40342A;
  color: #A79483;
  width: 36px;
  height: 36px;
  border-radius: 10px;
  font-size: 16px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background .15s, color .15s;
  flex-shrink: 0;
  margin-left: 12px;
}
.modal-close:hover { background: #40342A; color: #F3E9DE; }

.modal-body { padding: 20px 24px 24px; }

.stats-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 10px;
  margin-bottom: 18px;
}

.stat-card {
  display: flex;
  align-items: center;
  gap: 10px;
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 14px;
  padding: 12px 14px;
}

.stat-card.accent {
  border-color: rgba(224, 164, 88, 0.3);
  background: rgba(224, 164, 88, 0.06);
}

.stat-info { display: flex; flex-direction: column; min-width: 0; }
.stat-info small { font-size: 10px; color: #8A745E; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; }
.stat-info b { font-size: 15px; color: #F3E9DE; }

.score-breakdown {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 14px;
  padding: 14px;
  margin-bottom: 18px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.score-row {
  display: flex;
  align-items: center;
  gap: 10px;
}

.score-label { font-size: 12px; color: #A79483; font-weight: 600; width: 90px; flex-shrink: 0; }

.score-bar-wrap {
  flex: 1;
  height: 8px;
  background: #33291F;
  border-radius: 4px;
  overflow: hidden;
}

.score-bar {
  height: 100%;
  background: linear-gradient(90deg, #E0A458, #8FBF7E);
  border-radius: 4px;
  transition: width 0.6s cubic-bezier(.22,.68,0,1.1);
}

.score-bar.budget {
  background: linear-gradient(90deg, #E0A458, #D4A06A);
}

.score-value { font-size: 12px; color: #F3E9DE; font-weight: 700; width: 36px; text-align: right; }

.mini-map-section { margin-bottom: 18px; }
.mini-map {
  width: 100%;
  height: 160px;
  border-radius: 14px;
  overflow: hidden;
  border: 1px solid #33291F;
}

.favorite-section {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 14px;
  padding: 16px;
}

.fav-action-btn {
  width: 100%;
  padding: 10px;
  border: 1px solid #40342A;
  background: #2B231D;
  color: #A79483;
  border-radius: 10px;
  font-size: 13px;
  font-weight: 700;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all .2s;
}

.fav-action-btn:hover { border-color: #E0A458; color: #E0A458; }

.fav-action-btn.active {
  background: rgba(224, 164, 88, 0.1);
  border-color: #E0A458;
  color: #E0A458;
}

.fav-star { font-size: 16px; }

.fav-note-area {
  margin-top: 14px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.fav-note-area label {
  font-size: 11px;
  font-weight: 700;
  color: #8A745E;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.fav-note-area textarea {
  width: 100%;
  padding: 10px 12px;
  background: #2B231D;
  border: 1px solid #40342A;
  border-radius: 10px;
  color: #F3E9DE;
  font-family: 'Manrope', sans-serif;
  font-size: 13px;
  resize: vertical;
  min-height: 60px;
  outline: none;
  transition: border-color .2s;
  box-sizing: border-box;
}
.fav-note-area textarea:focus { border-color: #E0A458; }

.note-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.char-count { font-size: 11px; color: #5A4526; }

.save-note-btn {
  padding: 6px 16px;
  background: #E0A458;
  color: #17120F;
  border: none;
  border-radius: 8px;
  font-size: 12px;
  font-weight: 800;
  font-family: 'Manrope', sans-serif;
  cursor: pointer;
  transition: opacity .15s;
}
.save-note-btn:hover { opacity: 0.85; }
.save-note-btn:disabled { opacity: 0.5; cursor: not-allowed; }

@media (max-width: 520px) {
  .modal-overlay { padding: 0; align-items: flex-end; }
  .modal-card {
    max-width: 100%;
    max-height: 92vh;
    border-radius: 24px 24px 0 0;
    border-bottom: none;
  }
  .stats-grid { grid-template-columns: 1fr; }
}
</style>
