using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dythervin.Callbacks;
using Dythervin.Collections;
using Dythervin.Core.Extensions;
using Dythervin.Routines;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using Unity.Mathematics;
using UnityEngine;

namespace Dythervin.Game
{
    public struct Viewer
    {
        public Vector3 pos;
        public Vector3 forward;
        private float _dot;

        public float Angle
        {
            set => _dot = math.clamp((value - 180) / -180, -1, 1);
        }

        public Viewer(in Vector3 pos, in Vector3 forward, float angle) : this()
        {
            this.pos = pos;
            this.forward = forward;
            Angle = angle;
        }

        public bool InSight(Vector3 target)
        {
            target -= pos;
            float value = Vector3.Dot(target, forward);
            return value >= _dot;
        }
    }

    [RequireComponent(typeof(Collider))]
    public abstract class ObjectsRadar<T> : MonoBehaviour
    {
        private static readonly List<T> ToRemove = new List<T>();


        [SerializeField] [Range(0, 360)] private float angle = 180;
        [SerializeField] private bool raycast;
        [SerializeField] [Min(0.01f)] private float checkInterval = 0.3f;
#if ODIN_INSPECTOR
        [ShowIf(nameof(raycast))]
#endif
        [SerializeField]
        private Transform raycastSource;
#if ODIN_INSPECTOR
        [ShowIf(nameof(raycast))]
#endif
        [SerializeField]
        private LayerMask raycastMask;

        private readonly DictionaryCross<T, GameObject> _gameObjects = new DictionaryCross<T, GameObject>();

        private readonly HashSet<T> _objects = new HashSet<T>();
#if ODIN_INSPECTOR
        [ShowInInspector] [ReadOnly]
#endif
        private readonly HashSet<T> _objectsInSight = new HashSet<T>();
        private readonly FuncIn<T, Vector3> _objPosGetter;
        private readonly Action<ICallbacks> _removeObj;
        private readonly RoutineSeq _routineSeq = new RoutineSeq(true);

        public event Action<T> OnObjInRadius;
        public event Action<T> OnObjInSight;
        public event Action<T> OnObjLeftRadius;
        public event Action<T> OnObjLeftSight;
        public IReadOnlyCollection<T> Objects => _objects;
        public IReadOnlyCollection<T> ObjectsInSight => _objectsInSight;

        private Viewer Viewer => new Viewer(transform.position, transform.forward, angle);

        public ObjectsRadar()
        {
            _removeObj = other =>
            {
                if (other.gameObject.TryGetComponent(out T value))
                    Remove(value);
            };
            _objPosGetter = PosGetterFilter;
        }

        protected virtual Vector3 PosGetterFilter(in T value)
        {
            return _gameObjects[value].transform.position;
        }

        protected virtual void Awake() { }

        protected virtual void Start()
        {
            if (raycast || angle < 360)
            {
                _routineSeq.Append(new WaitForSecondsInstr(checkInterval, true));
                _routineSeq.Append(CheckAll);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetClosestInRadius(in Vector3 position, out T value)
        {
            return _objects.GetClosest(position, _objPosGetter, out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetClosestInSight(in Vector3 position, out T value)
        {
            return _objectsInSight.GetClosest(position, _objPosGetter, out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InSight(T obj)
        {
            return _objectsInSight.Contains(obj);
        }


        protected virtual bool IsSightFilter(T obj)
        {
            return true;
        }

        protected abstract bool IsValid(Collider other);

        private void OnTriggerEnter(Collider other)
        {
            if (!IsValid(other))
                return;

            GameObject gameObj = other.gameObject;
            if (!gameObj.TryGetComponent(out T obj))
                return;

            _objects.Add(obj);
            gameObj.GetCallbacks().OnDisabled += _removeObj;

            bool inSight = IsSightFilter(obj);

            if (inSight && angle < 360)
            {
                Viewer viewer = Viewer;
                Vector3 target = other.transform.position;
                if (viewer.InSight(target))
                    inSight = !raycast
                              || (Physics.Linecast(raycastSource.position, target, out RaycastHit hit, raycastMask, QueryTriggerInteraction.Ignore)
                                  && hit.collider.CompareTag(other.tag));
            }

            if (inSight)
                ObjInSight(obj);

            OnObjInRadius?.Invoke(obj);
            if (_objects.Count == 1)
                _routineSeq.Start();
        }

        private void OnTriggerExit(Collider other)
        {
            TryRemove(other.gameObject);
        }

        private void CheckAll()
        {
            if (_objects.Count == 0)
                return;

            ToRemove.Clear();
            Viewer viewer = Viewer;
            if (raycast)
            {
                Vector3 pos = raycastSource.position;
                foreach (T obj in _objectsInSight)
                {
                    GameObject gameObj = _gameObjects[obj];
                    Vector3 target = gameObj.transform.position;
                    if (IsSightFilter(obj)
                        && viewer.InSight(target)
                        && Physics.Linecast(pos, target, out RaycastHit hit, raycastMask, QueryTriggerInteraction.Ignore)
                        && hit.collider.CompareTag(gameObj.tag))
                        continue;

                    ToRemove.Add(obj);
                }

                foreach (T obj in _objects)
                {
                    if (_objectsInSight.Contains(obj))
                        continue;

                    GameObject gameObj = _gameObjects[obj];
                    Vector3 target = gameObj.transform.position;
                    if (!IsSightFilter(obj)
                        || !viewer.InSight(target)
                        || !Physics.Linecast(pos, target, out RaycastHit hit, raycastMask, QueryTriggerInteraction.Ignore)
                        || !hit.collider.CompareTag(gameObj.tag))
                        continue;

                    ObjInSight(obj);
                }
            }
            else
            {
                foreach (T obj in _objectsInSight)
                {
                    GameObject gameObj = _gameObjects[obj];
                    if (!IsSightFilter(obj) || !viewer.InSight(gameObj.transform.position))
                        ToRemove.Add(obj);
                }

                foreach (T obj in _objects)
                {
                    if (_objectsInSight.Contains(obj))
                        continue;


                    GameObject gameObj = _gameObjects[obj];
                    Vector3 target = gameObj.transform.position;
                    if (!IsSightFilter(obj) || !viewer.InSight(target))
                        continue;

                    ObjInSight(obj);
                }
            }

            if (ToRemove.Count > 0)
            {
                foreach (T obj in ToRemove)
                {
                    ObjLeftSight(obj);
                }

                ToRemove.Clear();
            }
        }

        private void ObjInSight(T obj)
        {
            if (_objectsInSight.Add(obj))
                OnObjInSight?.Invoke(obj);
        }

        private void ObjLeftSight(T obj)
        {
            if (_objectsInSight.Remove(obj))
                OnObjLeftSight?.Invoke(obj);
        }

        private void Remove(T obj)
        {
            _gameObjects.Pop(obj, out GameObject gameObj);
            gameObj.GetCallbacks().OnDisabled -= _removeObj;
            _objects.Remove(obj);
            ObjLeftSight(obj);

            OnObjLeftRadius?.Invoke(obj);
            if (_objects.Count == 0)
                _routineSeq.Stop();
        }


        private void TryRemove(GameObject gameObj)
        {
            if (_gameObjects.TryGetValue(gameObj, out T obj))
                Remove(obj);
        }
    }
}