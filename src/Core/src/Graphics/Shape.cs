using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public abstract class Shape : IShape
	{
		public abstract PathF CreatePath(RectangleF rect, float density = 1.0f);
	}
}