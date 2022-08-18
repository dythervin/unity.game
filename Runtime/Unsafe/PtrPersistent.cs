using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Dythervin.UnsafeUtils.Unsafe
{
    public readonly unsafe struct PtrPersistent<T> : IDisposable
        where T : struct
    {
        [NativeDisableUnsafePtrRestriction] private readonly void* _value;

        public bool IsNull()
        {
            return _value == null;
        }

        public PtrPersistent(ref T value)
        {
            _value = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), Allocator.Persistent);
            UnsafeUtility.CopyStructureToPtr(ref value, _value);
        }

        public T Value
        {
            get
            {
                UnsafeUtility.CopyPtrToStructure(_value, out T value);
                return value;
            }
        }

        public void GetValue(out T value)
        {
            UnsafeUtility.CopyPtrToStructure(_value, out value);
        }

        public void Dispose()
        {
            UnsafeUtility.Free(_value, Allocator.Persistent);
        }
    }
}