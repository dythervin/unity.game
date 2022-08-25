#if DYTHERVIN_LABELS
using UnityEngine;

namespace Dythervin.Game.Tags
{
    public static class Helper
    {
        public static bool CompareTag(this Component component, int tags)
        {
            var list = Constants.Tags.All;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((tags & i) == 0)
                    continue;
            
                if (component.CompareTag(list[i]))
                    return true;
            }

            return false;
        }
    }
}
#endif