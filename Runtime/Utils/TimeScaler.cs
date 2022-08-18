using Dythervin.Collections;
using Dythervin.PersistentData;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Dythervin.Game.Utils
{
    public class TimeScaler : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private SerializedDictionary<KeyCode, float> values;

        [SerializeField]
        private bool cache = true;

#if ODIN_INSPECTOR
        [ShowIf(nameof(cache))]
#endif
        [SerializeField]
        private string playerPrefKey;

        private Pref<float> _pref;

        private void Awake()
        {
            if (cache)
            {
                Prefs.Default.Get(playerPrefKey, out _pref);
                Time.timeScale = _pref;
            }
        }

        private void Update()
        {
            foreach (KeyCode key in values.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    float scale = values[key];
                    _pref.Value = scale == Time.timeScale ? 1 : scale;
                    Time.timeScale = _pref;
                    return;
                }
            }
        }
#endif
    }
}