<template>
  <!-- =========================================================================================
       RESTORAN İSKELET YÜKLEYİCİSİ (SKELETON LOADER) (KODLAMA BİLMEYENLER İÇİN ANLATIM)
       Restoranlar internetten indirilirken boş beyaz ekran göstermek yerine
       ışık parlamalı (shimmer) gri hayalet kartlar göstererek kullanıcının beklemesini keyifli kılar.
       ========================================================================================= -->
  <div class="skeleton-list">
    <div v-for="n in count" :key="n" class="skeleton-item">
      <div class="skeleton-thumb shimmer"></div>
      <div class="skeleton-body">
        <div class="skeleton-row space-between">
          <div class="skeleton-line title shimmer"></div>
          <div class="skeleton-badge shimmer"></div>
        </div>
        <div class="skeleton-line meta shimmer"></div>
        <div class="skeleton-line dist shimmer"></div>
        <div class="skeleton-pill shimmer"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
// =========================================================================================
// İSKELET PARAMETRELERİ (PROPS)
// =========================================================================================
defineProps({
  count: {
    type: Number,
    default: 4 // Varsayılan olarak 4 adet iskelet kart çizer
  }
})
</script>

<style scoped>
.skeleton-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.skeleton-item {
  display: flex;
  min-height: 84px;
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 15px;
  overflow: hidden;
  position: relative;
}

.skeleton-thumb {
  width: 74px;
  min-height: 84px;
  background: #2B231D;
  flex: 0 0 74px;
}

.skeleton-body {
  flex: 1;
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  justify-content: center;
}

.skeleton-row {
  display: flex;
  align-items: center;
}

.skeleton-row.space-between {
  justify-content: space-between;
}

.skeleton-line {
  background: #2B231D;
  border-radius: 6px;
}

.skeleton-line.title {
  height: 16px;
  width: 55%;
}

.skeleton-badge {
  height: 16px;
  width: 50px;
  background: #2B231D;
  border-radius: 6px;
}

.skeleton-line.meta {
  height: 12px;
  width: 40%;
}

.skeleton-line.dist {
  height: 11px;
  width: 65%;
}

.skeleton-pill {
  height: 20px;
  width: 110px;
  background: #2B231D;
  border-radius: 8px;
  margin-top: 2px;
}

.shimmer {
  position: relative;
  overflow: hidden;
}

.shimmer::after {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  transform: translateX(-100%);
  background-image: linear-gradient(
    90deg,
    rgba(255, 255, 255, 0) 0,
    rgba(224, 164, 88, 0.08) 20%,
    rgba(224, 164, 88, 0.18) 60%,
    rgba(255, 255, 255, 0)
  );
  animation: shimmer 1.8s infinite;
  content: '';
}

@keyframes shimmer {
  100% {
    transform: translateX(100%);
  }
}
</style>
