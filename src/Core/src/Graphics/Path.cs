using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public class Path : Shape
	{
		public Path()
		{

		}

		public Path(string? data)
		{
			Data = data;
		}

		public string? Data { get; set; }

		public override PathF CreatePath(RectangleF rect, float density = 1)
		{
			return PathBuilder.Build(Data);
		}
	}
}