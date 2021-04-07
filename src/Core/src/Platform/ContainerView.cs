using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public partial class ContainerView
	{
		private IShape? _clipShape;
		public IShape? ClipShape
		{
			get => _clipShape;
			set
			{
				if (_clipShape == value)
					return;

				_clipShape = value;
				ClipShapeChanged();
			}
		}

		partial void ClipShapeChanged();
	}
}