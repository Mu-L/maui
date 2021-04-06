using System.Graphics;
using System.Graphics.CoreGraphics;
using CoreGraphics;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static CGPath ToNative(this IShape shape, RectangleF rect, float density = 1.0f)
		{
			var pathF = shape.CreatePath(rect, density);
			return pathF.AsCGPath();
		}

		public static CGPath ToNative(this PathF pathF)
		{
			return pathF.AsCGPath();
		}
	}
}