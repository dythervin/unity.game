using System;
using System.Collections;
using Dythervin.Core.Utils;
using UnityEngine;

namespace Dythervin.Game
{
    public static class Inputs
    {
        [Flags]
        public enum InputPhase
        {
            None = 0,
            Began = 1 << 0,
            Moved = 1 << 1,
            Stationary = 1 << 2,
            Ended = 1 << 3,
            Canceled = 1 << 4
        }


        public static bool GetInput(in InputPhase phase)
        {
#if UNITY_EDITOR
            if (GetMouseInput(phase))
                return true;
#endif
            if (Input.touchCount == 0)
                return false;

            var touchPhase = Input.GetTouch(0).phase;

            switch (touchPhase)
            {
                case TouchPhase.Began:
                    return (phase & InputPhase.Began) != 0;
                case TouchPhase.Moved:
                    return (phase & InputPhase.Moved) != 0;
                case TouchPhase.Stationary:
                    return (phase & InputPhase.Stationary) != 0;
                case TouchPhase.Ended:
                    return (phase & InputPhase.Ended) != 0;
                // TouchPhase.Canceled => phase.HasFlag(InputPhase.Canceled),
                case TouchPhase.Canceled:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool GetInput(in InputPhase phase, out Vector3 position)
        {
#if UNITY_EDITOR
            if (GetMouseInput(phase))
            {
                position = Input.mousePosition;
                return true;
            }
#endif
            if (Input.touchCount == 0)
            {
                position = Vector3.zero;
                return false;
            }

            var touch = Input.GetTouch(0);
            position = touch.position;
            var touchPhase = touch.phase;

            switch (touchPhase)
            {
                case TouchPhase.Began:
                    return (phase & InputPhase.Began) != 0;
                case TouchPhase.Moved:
                    return (phase & InputPhase.Moved) != 0;
                case TouchPhase.Stationary:
                    return (phase & InputPhase.Stationary) != 0;
                case TouchPhase.Ended:
                    return (phase & InputPhase.Ended) != 0;
                case TouchPhase.Canceled:
                    return (phase & InputPhase.Canceled) != 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


#if UNITY_EDITOR
        private static bool GetMouseInput(in InputPhase phase)
        {
            if ((phase & InputPhase.Began) != 0 && Input.GetMouseButtonDown(0))
                return true;

            if ((phase & InputPhase.Ended) != 0 && Input.GetMouseButtonUp(0))
                return true;

            if ((phase & InputPhase.Stationary) != 0 && Input.GetMouseButton(0) && !_inputMoved)
                return true;

            if ((phase & InputPhase.Moved) != 0 && Input.GetMouseButton(0) && _inputMoved)
                return true;

            return false;
        }
#endif
#if UNITY_EDITOR
        private static Vector3 _previousMousePos;
        private static bool _inputMoved;
#endif


#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void TryInit()
        {
            CoroutineRunner.Instance.StartCoroutine(UpdateMousePosition());
        }

        private static IEnumerator UpdateMousePosition()
        {
            while (true)
            {
                Vector3 currentPos = Input.mousePosition;
                _inputMoved = currentPos != _previousMousePos;
                _previousMousePos = currentPos;
                yield return null;
            }
        }
#endif
    }
}