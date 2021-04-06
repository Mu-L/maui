using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public class Ellipse : Shape
	{
		public Ellipse()
		{
			Stretch = Stretch.Fill;
		}

		public override PathF CreatePath(RectangleF rect, float density = 1)
		{
			var path = new PathF();

			path.AppendEllipse(rect);

			return path;
		}
	}
}