using System.Graphics;
using System.Graphics.Android;
using Microsoft.Maui.Graphics;
using APath = Android.Graphics.Path;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static APath ToNative(this IShape shape, RectangleF rect, float density = 1.0f)
		{
			var pathF = shape.CreatePath(rect, density);
			return pathF.AsAndroidPath();
		}

		public static APath ToNative(this PathF pathF)
		{
			return pathF.AsAndroidPath();
		}
	}
}