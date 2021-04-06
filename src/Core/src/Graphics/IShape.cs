using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	/// <summary>
	/// Provides a base definition class for shape elements, such as
	/// Ellipse, Polygon, or Rectangle.
	/// </summary>
	public interface IShape
	{
		/// <summary>
		/// Define the PathF used to draw a specific Shape.
		/// </summary>
		/// <param name="rect">Define the shape size and position.</param>
		/// <param name="density">A virtual unit of measure to allow layouts to be designed independent of density.</param>
		/// <returns>The PathF used to draw a specific Shape</returns>
		PathF CreatePath(RectangleF rect, float density = 1.0f);
	}
}