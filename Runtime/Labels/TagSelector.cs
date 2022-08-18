using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.Game.Tags
{
    public class TagSelector : MonoBehaviour
    {
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ValueDropdown(nameof(Values))]
#endif
        public List<string> tags;

        public bool IsValid(Component other)
        {
            foreach (string tag in tags)
            {
                if (other.CompareTag(tag))
                    return true;
            }

            return false;
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        public static readonly ValueDropdownList<string> Values;

        static TagSelector()
        {
            Values = new ValueDropdownList<string>();
            foreach (string tag in Constants.Tags.All)
            {
                Values.Add(tag, tag);
            }
        }

        public const string ValuesGetter = "@" + nameof(Core) + "." + nameof(Constants) + "." + nameof(TagSelector) + "." + nameof(Values);
#endif
    }
}