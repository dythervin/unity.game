using Unity.Mathematics;
using UnityEngine;

namespace Dythervin.Game.Utils
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
    }
}