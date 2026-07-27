import { ref, computed } from 'vue'
import tr from '~/locales/tr.json'
import en from '~/locales/en.json'

const currentLocale = ref('tr')
const dictionaries: Record<string, any> = { tr, en }

export const useI18n = () => {
  const locale = computed({
    get: () => currentLocale.value,
    set: (val: string) => { currentLocale.value = val }
  })

  const setLocale = (lang: string) => {
    currentLocale.value = lang
  }

  const t = (path: string, params: Record<string, any> = {}) => {
    const dict = dictionaries[currentLocale.value] || tr
    const keys = path.split('.')
    let res = dict
    for (const k of keys) {
      if (res && res[k] !== undefined) {
        res = res[k]
      } else {
        return path
      }
    }
    if (typeof res === 'string') {
      let str = res
      for (const [pKey, pVal] of Object.entries(params)) {
        str = str.replace(`%{${pKey}}`, String(pVal))
      }
      return str
    }
    return res
  }

  return {
    locale,
    setLocale,
    t
  }
}
