using UnityEngine;

namespace Dythervin.Game.Common
{
    public class RotatorFixedUpdate : MonoBehaviour
    {
        [SerializeField] private Vector3 eulers;

#if !UNITY_EDITOR
        private void Awake()
        {
            eulers *= Time.fixedDeltaTime;
        }
#endif

        private void FixedUpdate()
        {
            transform.Rotate(
#if UNITY_EDITOR
                Time.fixedDeltaTime *
#endif
                eulers
            );
        }
    }
}