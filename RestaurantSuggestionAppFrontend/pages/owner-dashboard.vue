<template>
  <div class="owner-page">
    <header class="owner-header">
      <div class="hbar">
        <div>
          <span class="logo">Sofra</span>
          <span class="badge">Restoran Sahibi Paneli</span>
        </div>
        <div class="right-actions">
          <span class="user-name">Hoş Geldiniz, <strong>{{ authStore.user?.name }}</strong></span>
          <button @click="authStore.logout()" class="btn-logout">Çıkış Yap</button>
        </div>
      </div>
    </header>

    <main class="dashboard-container">
      <div v-if="isLoading" class="loading">İstatistikler Yükleniyor...</div>
      <div v-else-if="errorMessage" class="error-msg">{{ errorMessage }}</div>

      <template v-else>
        <!-- Metrik Kartları -->
        <div class="metrics-grid">
          <div class="metric-card gold">
            <span class="m-lbl">İşletme Bilgisi</span>
            <div class="m-val" style="font-size: 24px;">Restoran #{{ authStore.user?.restaurantId || '1' }}</div>
            <div class="m-sub">Restoran Sahibi Yönetim Paneli</div>
          </div>

          <div class="metric-card">
            <span class="m-lbl">Sistem Durumu</span>
            <div class="m-val" style="font-size: 24px;">Aktif</div>
            <div class="m-sub">Restoranınız müşterilerin haritasında görüntüleniyor</div>
          </div>
        </div>
      </template>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '~/stores/authStore'

const authStore = useAuthStore()
const isLoading = ref(false)
const errorMessage = ref('')
</script>

<style scoped>
.owner-page {
  min-height: 100vh;
  background-color: #17120F;
  color: #F3E9DE;
  font-family: 'Manrope', sans-serif;
  padding-bottom: 40px;
}

.owner-header {
  border-bottom: 1px solid #33291F;
  padding: 16px 32px;
  background: #201A16;
}

.hbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1100px;
  margin: 0 auto;
}

.logo {
  font-family: 'Instrument Serif', serif;
  font-size: 28px;
  font-style: italic;
  color: #E0A458;
  margin-right: 12px;
}

.badge {
  background: rgba(224, 164, 88, 0.15);
  color: #E0A458;
  border: 1px solid rgba(224, 164, 88, 0.3);
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 11px;
  font-weight: 700;
  text-transform: uppercase;
}

.right-actions {
  display: flex;
  align-items: center;
  gap: 16px;
  font-size: 13px;
}

.btn-logout {
  background: rgba(224, 113, 106, 0.15);
  color: #E0716A;
  border: 1px solid rgba(224, 113, 106, 0.3);
  padding: 6px 14px;
  border-radius: 8px;
  font-size: 12px;
  cursor: pointer;
}

.dashboard-container {
  max-width: 1100px;
  margin: 36px auto;
  padding: 0 24px;
}

.loading { text-align: center; color: #A79483; padding: 40px; }
.error-msg { background: rgba(224, 113, 106, 0.15); border: 1px solid #E0716A; color: #E0716A; padding: 16px; border-radius: 12px; }

.metrics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 20px;
  margin-bottom: 24px;
}

.metric-card {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 16px;
  padding: 24px;
}
.metric-card.gold { border-color: rgba(224, 164, 88, 0.4); }

.m-lbl { font-size: 11px; color: #A79483; text-transform: uppercase; font-weight: 700; }
.m-val { font-size: 32px; font-weight: 800; color: #F3E9DE; margin: 8px 0; }
.metric-card.gold .m-val { color: #E0A458; }
.m-sub { font-size: 12px; color: #8A745E; }

/* QR Creator Card */
.qr-creator-card {
  background: #201A16;
  border: 1px solid #40342A;
  border-radius: 20px;
  padding: 24px;
  margin-bottom: 30px;
}

.qr-creator-header h2 {
  font-size: 18px;
  margin: 0 0 4px;
  color: #F3E9DE;
}
.qr-creator-header p {
  font-size: 12.5px;
  color: #A79483;
  margin: 0 0 20px;
}

.qr-form-row {
  display: flex;
  gap: 16px;
  align-items: flex-end;
}

.flex-1 { flex: 1; }

.custom-input {
  width: 100%;
  background: #2B231D;
  border: 1px solid #40342A;
  border-radius: 12px;
  padding: 13px 15px;
  color: #F3E9DE;
  font-size: 14px;
  outline: none;
}
.custom-input:focus { border-color: #E0A458; }

.btn-primary-qr {
  background: #E0A458;
  color: #201A16;
  border: none;
  padding: 14px 24px;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 800;
  cursor: pointer;
  white-space: nowrap;
}
.btn-primary-qr:disabled { opacity: 0.5; cursor: not-allowed; }

.created-qr-box {
  background: #17120F;
  border: 1px solid #E0A458;
  border-radius: 16px;
  padding: 20px;
  margin-top: 20px;
  transition: all 0.3s ease;
}

.created-qr-box.paid-box {
  border-color: #52c41a;
  background: rgba(82, 196, 26, 0.08);
}

.qr-display {
  display: flex;
  align-items: center;
  gap: 24px;
}

.qr-img-wrapper {
  background: #fff;
  padding: 10px;
  border-radius: 14px;
  flex-shrink: 0;
}

.qr-code-img {
  width: 180px;
  height: 180px;
  display: block;
}

.qr-info-side { flex: 1; }

.qr-status-badge {
  font-size: 12px;
  font-weight: 700;
  color: #E0A458;
  margin-bottom: 8px;
}

.qr-status-badge.paid {
  color: #52c41a;
  font-size: 14px;
}

.text-success {
  color: #52c41a !important;
}

.qr-amount-large {
  font-size: 34px;
  font-weight: 800;
  color: #E0A458;
}

.qr-token-text {
  font-size: 12px;
  color: #C9B9A8;
  margin: 8px 0;
}
.qr-token-text code {
  background: #2B231D;
  padding: 3px 8px;
  border-radius: 6px;
  color: #E0A458;
  font-family: monospace;
}

.qr-hint {
  font-size: 12px;
  color: #8A745E;
  margin: 0;
  line-height: 1.4;
}

.table-card {
  background: #201A16;
  border: 1px solid #33291F;
  border-radius: 16px;
  padding: 24px;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}
.table-header h2 { font-size: 18px; margin: 0; color: #F3E9DE; }

.btn-refresh {
  background: #2B231D;
  color: #A79483;
  border: 1px solid #40342A;
  padding: 6px 12px;
  border-radius: 8px;
  font-size: 12px;
  cursor: pointer;
}

.empty-text { color: #8A745E; font-size: 13px; text-align: center; padding: 20px; }

.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13.5px;
}

.data-table th {
  text-align: left;
  padding: 12px;
  border-bottom: 1px solid #33291F;
  color: #A79483;
  font-size: 11px;
  text-transform: uppercase;
}

.data-table td {
  padding: 14px 12px;
  border-bottom: 1px solid #2B231D;
  color: #F3E9DE;
}

.amount-cell { color: #E0A458; font-weight: 700; }
.date-cell { color: #8A745E; font-size: 12px; }

.status-badge {
  background: rgba(82, 196, 26, 0.15);
  color: #52c41a;
  border: 1px solid rgba(82, 196, 26, 0.3);
  padding: 3px 8px;
  border-radius: 6px;
  font-size: 11px;
  font-weight: 700;
}
</style>
