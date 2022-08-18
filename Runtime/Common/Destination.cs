using UnityEngine;

namespace Dythervin.Game.Common
{
    public class Destination : MonoBehaviour, IRadiused
    {
        [SerializeField, Min(0)] private float radius = 0.5f;

        public float Radius => radius;
    }
}