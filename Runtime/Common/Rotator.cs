using UnityEngine;

namespace Dythervin.Game.Common
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 eulers;

        private void Update()
        {
            transform.Rotate(eulers * Time.deltaTime);
        }
    }
}