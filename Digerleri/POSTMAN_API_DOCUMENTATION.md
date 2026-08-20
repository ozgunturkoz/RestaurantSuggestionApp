# 🚀 Restaurant Suggestion App - Postman API Dokümantasyonu

Bu doküman, **Akıllı Restoran Öneri Sistemi (Restaurant Suggestion App)** projesinin tüm RESTful API uç noktalarını (endpoints), istek/yanıt şemalarını ve Postman ile nasıl test edilip kullanılacağını açıklar.

---

## 📌 İçindekiler
1. [Hızlı Başlangıç ve Postman'e Aktarma](#-hızlı-başlangıç-ve-postmane-aktarma)
2. [Yetkilendirme (JWT & 2FA)](#-yetkilendirme-jwt--2fa)
3. [API Uç Noktaları (Endpoints)](#-api-uç-noktaları-endpoints)
   - 🔑 [Auth (Kimlik Doğrulama)](#1-auth-kimlik-doğrulama)
   - 🍽️ [Restaurants (Restoran İşlemleri)](#2-restaurants-restoran-işlemleri)
   - 👤 [Users & Profile (Kullanıcı İşlemleri)](#3-users--profile-kullanıcı-işlemleri)
   - ⭐ [Favorites (Favori İşlemleri)](#4-favorites-favori-işlemleri)
   - 💳 [Expenditures (Harcama Takibi)](#5-expenditures-harcama-takibi)
   - 🔒 [Account Security (Şifre & E-posta Güvenliği)](#6-account-security-şifre--e-posta-güvenliği)
   - 🌐 [Localization (Dil ve Yerelleştirme)](#7-localization-dil-ve-yerelleştirme)

---

## 📥 Hızlı Başlangıç ve Postman'e Aktarma

Projede hazır oluşturulmuş olan Postman Koleksiyon dosyasını doğrudan Postman'e aktarabilirsiniz:

1. **Postman** uygulamasını açın.
2. Sol üstteki **Import** butonuna tıklayın.
3. Projedeki şu dosyayı seçin:
   ```text
   postman/collections/RestaurantSuggestionApp.postman_collection.json
   ```
4. Koleksiyon yüklendikten sonra `baseUrl` ortam değişkenini kontrol edin (Varsayılan: `https://localhost:7085`).

---

## 🔑 Yetkilendirme (JWT & 2FA)

Sistemdeki korumalı uç noktalar **Bearer Token (JWT)** yetkilendirmesi kullanır.

* **Otomatik Token Kaydı:** Postman koleksiyonundaki `POST /api/Auth/login` isteği çalıştırıldığında, dönen JWT token Postman tarafından otomatik olarak yakalanır ve `{{bearer_token}}` koleksiyon değişkenine yazılır.
* Diğer tüm istekler bu değişkeni otomatik başlık (`Authorization: Bearer {{bearer_token}}`) olarak gönderir.

---

## 🌐 API Uç Noktaları (Endpoints)

### 1. Auth (Kimlik Doğrulama)

#### `POST /api/Auth/login`
Kullanıcı e-posta ve şifre ile giriş yapar.

* **Headers:** `Content-Type: application/json`
* **Request Body:**
```json
{
  "email": "testuser@example.com",
  "password": "Password123!"
}
```
* **Success Response (200 OK):**
```json
{
  "isSuccess": true,
  "message": "Giriş başarılı.",
  "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### `POST /api/Auth/verify-login`
2FA doğrulaması için e-postaya gelen doğrulama kodunu onaylar.

* **Request Body:**
```json
{
  "email": "testuser@example.com",
  "code": "123456"
}
```

#### `POST /api/Auth/register`
Sisteme yeni kullanıcı kaydı oluşturur.

* **Request Body:**
```json
{
  "name": "Test Kullanıcı",
  "email": "testuser@example.com",
  "password": "Password123!"
}
```

---

### 2. Restaurants (Restoran İşlemleri)

> 🔒 **Auth Required:** `Bearer Token`

#### `GET /api/Restaurant`
Sistemdeki tüm restoranları listeler.

#### `GET /api/Restaurant/{id}`
Belirtilen ID'li restoranın detayını getirir.

#### `GET /api/Restaurant/nearby`
Verilen enlem/boylam konumuna belirtilen km yarıçapındaki en yakın ve önerilen restoranları hesaplayarak getirir.

* **Query Parameters:**
  - `latitude` (decimal, zorunlu) - Örn: `41.0082`
  - `longitude` (decimal, zorunlu) - Örn: `28.9784`
  - `radiusInKm` (double, opsiyonel, varsayılan `5`) - Örn: `5`

#### `POST /api/Restaurant`
Yeni restoran kaydı ekler.

* **Request Body:**
```json
{
  "name": "Lezzet Restoranı",
  "category": "Kebap",
  "latitude": 41.0082,
  "longitude": 28.9784,
  "averagePrice": 250.00
}
```

#### `PUT /api/Restaurant/{id}`
Restoran bilgilerini günceller.

#### `DELETE /api/Restaurant/{id}`
Restoran kaydını siler.

---

### 3. Users & Profile (Kullanıcı İşlemleri)

> 🔒 **Auth Required:** `Bearer Token`

#### `GET /api/User/me`
Giriş yapan kullanıcının kendi profil bilgilerini getirir.

#### `PUT /api/User/profile`
Giriş yapmış kullanıcının profil adını günceller.

* **Request Body:**
```json
{
  "name": "Özgür Türköz"
}
```

#### `GET /api/User` & `GET /api/User/{id}`
Tüm kullanıcıları veya ID'ye göre kullanıcı detayını getirir.

---

### 4. Favorites (Favori İşlemleri)

> 🔒 **Auth Required:** `Bearer Token`

#### `GET /api/User/favorites`
Giriş yapan kullanıcının favori restoranlarını listeler.

#### `POST /api/User/favorites`
Restoranı favorilere ekler.

* **Request Body:**
```json
{
  "restaurantId": 1,
  "customNote": "Sürekli tercih ettiğim mekan"
}
```

#### `DELETE /api/User/favorites/{restaurantId}`
Restoranı favorilerden çıkarır.

---

### 5. Expenditures (Harcama Takibi)

> 🔒 **Auth Required:** `Bearer Token`

#### `GET /api/User/expenditures`
Kullanıcının restoran harcama kayıtlarını getirir.

#### `POST /api/User/expenditures`
Yeni harcama kaydı ekler.

* **Request Body:**
```json
{
  "restaurantId": 1,
  "amount": 350.50,
  "description": "İş yemeği"
}
```

#### `DELETE /api/User/expenditures/{id}`
Harcama kaydını siler.

---

### 6. Account Security (Şifre & E-posta Güvenliği)

> 🔒 **Auth Required:** `Bearer Token`

#### `POST /api/User/send-password-code`
Şifre sıfırlama doğrulama kodunu e-postaya gönderir.

#### `POST /api/User/change-password-with-code`
Kod ile yeni şifreyi belirler.

* **Request Body:**
```json
{
  "code": "123456",
  "newPassword": "NewPassword123!"
}
```

#### `POST /api/User/send-email-change-code`
Yeni e-posta adresine doğrulama kodu gönderir.

#### `POST /api/User/change-email-with-code`
Kod ve mevcut şifre ile e-posta adresini günceller.

* **Request Body:**
```json
{
  "newEmail": "yeni.eposta@example.com",
  "code": "123456",
  "currentPassword": "Password123!"
}
```

---

### 7. Localization (Dil ve Yerelleştirme)

#### `GET /api/Localization?lang=tr` veya `GET /api/Localization/{lang}`
Sistemdeki yerelleştirilmiş dil anahtarlarını ve çevirilerini JSON olarak getirir (Örn: `tr`, `en`).

---

## 🛠️ Postman İpuçları
* **Postman Web Viewer / Documentation:** Postman masaüstü uygulamasında koleksiyon adının yanındaki `...` menüsünden **"View Documentation"** seçerek görsel olarak inceleyebilir veya **"Publish Documentation"** seçeneği ile web ortamında yayınlayabilirsiniz.
