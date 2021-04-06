using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public class Polygon : Shape
	{
		public Polygon()
		{

		}

		public Polygon(PointCollection? points)
		{
			Points = points;
		}

		public PointCollection? Points { get; set; }

		public override PathF CreatePath(RectangleF rect, float density = 1)
		{
			var path = new PathF();

			if (Points?.Count > 0)
			{
				path.MoveTo(density * (float)Points[0].X, density * (float)Points[0].Y);

				for (int index = 1; index < Points.Count; index++)
					path.LineTo(density * (float)Points[index].X, density * (float)Points[index].Y);

				path.Close();
			}

			return path;
		}
	}
}