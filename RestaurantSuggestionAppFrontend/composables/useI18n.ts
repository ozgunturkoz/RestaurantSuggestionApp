import { ref, computed } from 'vue'
import trFallback from '~/locales/tr.json'
import enFallback from '~/locales/en.json'

// =========================================================================================
// BU COMPOSABLE (KULLANILABİLİR FONKSİYON) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Bu dosya, web sitemizin "Tercümanıdır".
// 1. Kullanıcının seçtiği dili (TR veya EN) tarayıcının hafızasında (localStorage) hatırlar.
// 2. Sayfada yazılacak her metin için t('anahtar') fonksiyonu çağrıldığında, o anki dile
//    uygun olan Türkçe veya İngilizce karşılığını anında ekrana basar.
// 3. Backend sunucusundaki veritabanı sözlüğünden çevirileri çeker; internet olmasa bile
//    yerel tr.json ve en.json dosyalarını yedek olarak kullanarak sitenin asla bozulmamasını sağlar.
// =========================================================================================

// Yedek yerel çeviri sözlükleri
const fallbacks: Record<string, any> = {
  tr: trFallback,
  en: enFallback
}

// O an seçili olan aktif dil (varsayılan: Türkçe "tr")
const currentLocale = ref('tr')

// Backend'den çekilen dinamik sözlüklerin tutulduğu havuz
const dictionaries = ref<Record<string, Record<string, string>>>({})

// Dil yüklenirken dönen çark durumları
const isLoading = ref(false)
const isLoaded = ref(false)

export const useI18n = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase

  // -------------------------------------------------------------------------------------
  // Backend'den seçilen dilin sözlüğünü indiren fonksiyon
  // -------------------------------------------------------------------------------------
  const loadDictionary = async (lang: string) => {
    try {
      isLoading.value = true
      const data: Record<string, string> = await $fetch(`${apiBase}/api/localization?lang=${lang}`)
      if (data && typeof data === 'object' && Object.keys(data).length > 0) {
        dictionaries.value[lang] = data
      } else {
        // Backend boş dönerse yerel JSON dosyalarını devreye al
        dictionaries.value[lang] = fallbacks[lang] || trFallback
      }
    } catch {
      // Backend'e ulaşılamazsa sessizce yerel JSON sözlüklerini kullan
      dictionaries.value[lang] = fallbacks[lang] || trFallback
    } finally {
      isLoading.value = false
      isLoaded.value = true
    }
  }

  // -------------------------------------------------------------------------------------
  // Dili değiştiren fonksiyon (Kullanıcı TR veya EN butonuna bastığında çağrılır)
  // -------------------------------------------------------------------------------------
  const setLocale = async (lang: string) => {
    currentLocale.value = lang
    if (process.client) {
      // Kullanıcının tercihini tarayıcı hafızasına kaydediyoruz
      localStorage.setItem('app_lang', lang)
    }
    await loadDictionary(lang)
  }

  // -------------------------------------------------------------------------------------
  // Sayfa ilk açıldığında kullanıcının daha önceki dil tercihini hatırlayan fonksiyon
  // -------------------------------------------------------------------------------------
  const initLocale = async () => {
    if (process.client) {
      const savedLang = localStorage.getItem('app_lang')
      if (savedLang && (savedLang === 'tr' || savedLang === 'en')) {
        currentLocale.value = savedLang
      }
    }
    await loadDictionary(currentLocale.value)
  }

  // -------------------------------------------------------------------------------------
  // Seçili dili okuma ve ayarlama için reaktif computed özellik
  // -------------------------------------------------------------------------------------
  const locale = computed({
    get: () => currentLocale.value,
    set: (val: string) => { setLocale(val) }
  })

  // -------------------------------------------------------------------------------------
  // ANA ÇEVİRİ FONKSİYONU: t('profile.myProfile') gibi çağrılır.
  // Parametre desteği de vardır: t('badge.recommendation', { score: 95 }) -> "%95 Multinet profilinize uygun"
  // -------------------------------------------------------------------------------------
  const t = (path: string, params: Record<string, any> = {}) => {
    const activeDict = dictionaries.value[currentLocale.value] || fallbacks[currentLocale.value] || trFallback
    
    // 1. Önce düz anahtar kontrolü (örneğin veritabanından gelen "profile.myProfile")
    let val: any = activeDict ? activeDict[path] : undefined
    
    // 2. Düz anahtarda yoksa iç içe geçmiş nesne yapısında ara (JSON yapısı için)
    if (val === undefined && activeDict) {
      const keys = path.split('.')
      let current: any = activeDict
      for (const k of keys) {
        if (current && current[k] !== undefined) {
          current = current[k]
        } else {
          current = undefined
          break
        }
      }
      val = current
    }

    // 3. Bulunamazsa yerel fallback dosyasına başvur
    if (val === undefined) {
      const fallbackDict = fallbacks[currentLocale.value] || trFallback
      val = fallbackDict ? fallbackDict[path] : undefined
      if (val === undefined && fallbackDict) {
        const keys = path.split('.')
        let current: any = fallbackDict
        for (const k of keys) {
          if (current && current[k] !== undefined) {
            current = current[k]
          } else {
            current = undefined
            break
          }
        }
        val = current
      }
    }

    // Hiçbir yerde bulunamazsa anahtarın kendi adını ekranda göster
    if (val === undefined) {
      return path
    }

    // Değişken parametreleri metnin içine yerleştir (Örn: {count} veya %{score})
    if (typeof val === 'string') {
      let str = val
      for (const [pKey, pVal] of Object.entries(params)) {
        str = str.replace(`%{${pKey}}`, String(pVal)).replace(`{${pKey}}`, String(pVal))
      }
      return str
    }

    return val
  }

  return {
    locale,
    setLocale,
    initLocale,
    loadDictionary,
    t,
    isLoading,
    isLoaded
  }
}
