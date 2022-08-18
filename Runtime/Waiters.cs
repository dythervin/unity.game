using System.Collections.Generic;
using UnityEngine;

namespace Dythervin.Game
{
    public static class Waiters
    {
        private const int Capacity = 128;
        
        private static readonly Dictionary<float, WaitForSeconds> Scaled = new Dictionary<float, WaitForSeconds>(Capacity);
        private static readonly Dictionary<float, WaitForSecondsRealtime> Unscaled = new Dictionary<float, WaitForSecondsRealtime>(Capacity);

        public static readonly WaitForFixedUpdate FixedUpdate = new WaitForFixedUpdate();
        public static readonly WaitForEndOfFrame EndFrame = new WaitForEndOfFrame();


        public static WaitForSeconds Wait(float value, bool cache = true)
        {
            if (value <= 0)
                return null;

            if (!Scaled.TryGetValue(value, out WaitForSeconds wait))
            {
                wait = new WaitForSeconds(value);
                if (cache)
                    Scaled.Add(value, wait);
            }

            return wait;
        }

        public static WaitForSecondsRealtime WaitUnscaled(float value, bool cache = true)
        {
            if (value <= 0)
                return null;

            if (!Unscaled.TryGetValue(value, out WaitForSecondsRealtime wait))
            {
                wait = new WaitForSecondsRealtime(value);
                if (cache)
                    Unscaled.Add(value, wait);
            }

            return wait;
        }
    }
}