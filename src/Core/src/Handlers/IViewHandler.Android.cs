using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public interface INativeViewHandler : IViewHandler
	{
		AView? View { get; }
	}
}