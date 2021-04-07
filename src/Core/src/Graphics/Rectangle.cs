using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public class Rectangle : Shape
	{
		public Rectangle()
		{
			
		}

		public Rectangle(CornerRadius cornerRadius) : this()
		{
			CornerRadius = cornerRadius;
		}

		public CornerRadius CornerRadius { get; set; }

		public override PathF CreatePath(RectangleF rect, float density = 1)
		{
			var path = new PathF();

			path.AppendRoundedRectangle(
				rect,
				(float)CornerRadius.TopLeft,
				(float)CornerRadius.TopRight,
				(float)CornerRadius.BottomLeft,
				(float)CornerRadius.BottomRight);

			return path;
		}
	}
}