using System;
using UnityEngine;

namespace Dythervin.Game.Common
{
    public class ScalerSmooth : MonoBehaviour
    {
        [SerializeField, Min(0.0001f)] private float duration = 0.25f;

        private Vector3 currentScale;
        private Vector3 target;
        private float t;


        public void SetTargetScale(float value)
        {
            if (Math.Abs(target.x - value) < 0.01f)
                return;

            currentScale = transform.localScale;
            if (Math.Abs(target.x - value) < 0.01f)
                return;

            target = Vector3.one * value;
            t = 0;
            enabled = true;
        }

        private void Update()
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(currentScale, target, t);
            if (t >= 1)
                enabled = false;
        }

        private void Reset()
        {
            enabled = false;
        }
    }
}