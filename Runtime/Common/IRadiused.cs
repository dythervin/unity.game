using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Dythervin.Game.Common
{
    public interface IComponent
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")] Transform transform { get; }
    }

    public interface IRadiused : IComponent
    {
        float Radius { get; }
    }
}