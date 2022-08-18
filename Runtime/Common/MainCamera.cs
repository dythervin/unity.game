using UnityEngine;

namespace Dythervin.Game.Common
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        public static Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = cam;
        }
    }
}