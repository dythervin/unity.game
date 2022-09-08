using Dythervin.Core.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace Dythervin.Core.Utils
{
    public static class Vector3Helper
    {
        public static float GetPosition(int index, int count, float width)
        {
            if (count == 1)
                return 0;

            float totalWidth = width * (count - 1);

            float tLerp = math.lerp(-1, 1, index / (count - 1f));

            return tLerp * (totalWidth / 2);
        }

        public static bool GetPos(in Vector3 a, in Vector3 aV, in Vector3 b, float speed, out Vector3 pos, out float time)
        {
            time = (a - b).magnitude / (speed - aV.magnitude);
            pos = a + aV * time;
            return time >= 0;
        }
    }
}