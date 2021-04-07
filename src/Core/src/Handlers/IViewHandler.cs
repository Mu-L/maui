namespace Microsoft.Maui
{
	public interface IViewHandler
	{
		void SetMauiContext(IMauiContext mauiContext);
		void SetVirtualView(IView view);
		void UpdateValue(string property);
		void DisconnectHandler();
		object? NativeView { get; }
		bool HasContainer { get; set; }
		ContainerView? ContainerView { get; }
		Size GetDesiredSize(double widthConstraint, double heightConstraint);
		void SetFrame(Rectangle frame);
	}
}