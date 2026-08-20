import { ref, computed } from 'vue'
import trFallback from '~/locales/tr.json'
import enFallback from '~/locales/en.json'

const fallbacks: Record<string, any> = {
  tr: trFallback,
  en: enFallback
}

const currentLocale = ref('tr')
const dictionaries = ref<Record<string, Record<string, string>>>({})
const isLoading = ref(false)
const isLoaded = ref(false)

export const useI18n = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase

  const loadDictionary = async (lang: string) => {
    try {
      isLoading.value = true
      const data: Record<string, string> = await $fetch(`${apiBase}/api/localization?lang=${lang}`)
      if (data && typeof data === 'object' && Object.keys(data).length > 0) {
        dictionaries.value[lang] = data
      } else {
        // Fallback to local dictionary if backend returned empty
        dictionaries.value[lang] = fallbacks[lang] || trFallback
      }
    } catch (err) {
      console.warn(`[i18n] Backend localization fetch failed for '${lang}'. Using local fallback dictionary.`, err)
      // Secondary/Backup: Local JSON files
      dictionaries.value[lang] = fallbacks[lang] || trFallback
    } finally {
      isLoading.value = false
      isLoaded.value = true
    }
  }

  const setLocale = async (lang: string) => {
    currentLocale.value = lang
    if (process.client) {
      localStorage.setItem('app_lang', lang)
    }
    await loadDictionary(lang)
  }

  const initLocale = async () => {
    if (process.client) {
      const savedLang = localStorage.getItem('app_lang')
      if (savedLang && (savedLang === 'tr' || savedLang === 'en')) {
        currentLocale.value = savedLang
      }
    }
    await loadDictionary(currentLocale.value)
  }

  const locale = computed({
    get: () => currentLocale.value,
    set: (val: string) => { setLocale(val) }
  })

  const t = (path: string, params: Record<string, any> = {}) => {
    // 1. Primary: Backend dictionary
    // 2. Backup/Fallback: Local JSON files
    const activeDict = dictionaries.value[currentLocale.value] || fallbacks[currentLocale.value] || trFallback
    
    // First check flat dot-notation key (e.g. "nav.discover")
    let val: any = activeDict ? activeDict[path] : undefined
    
    // If not found in flat key, try nested object path (for local fallback JSONs)
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

    // Secondary Backup: If key still not found in active dict, check local fallbacks
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

    if (val === undefined) {
      return path
    }

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
