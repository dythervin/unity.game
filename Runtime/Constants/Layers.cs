//This class is auto-generated do not modify (TagsLayersScenesBuilder.cs) - blog.almostlogical.com

using UnityEngine;

namespace Dythervin.Game.Constants
{
	public static class Layers
	{
		public static class Default{
			public static readonly string name = "Default";
			public const int Int = 0;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class TransparentFX{
			public static readonly string name = "TransparentFX";
			public const int Int = 1;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class IgnoreRaycast{
			public static readonly string name = "Ignore Raycast";
			public const int Int = 2;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class Water{
			public static readonly string name = "Water";
			public const int Int = 4;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class UI{
			public static readonly string name = "UI";
			public const int Int = 5;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class TempCast{
			public static readonly string name = "TempCast";
			public const int Int = 8;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}

		public static class Walkable{
			public static readonly string name = "Walkable";
			public const int Int = 9;
			public static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};
		}
	}
}