using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Graphics;
using System.Graphics.Android;
using APath = Android.Graphics.Path;

namespace Microsoft.Maui
{
	public partial class ContainerView : FrameLayout
	{
		View? _mainView;
		APath? _currentPath;
		SizeF _lastPathSize;

		public ContainerView(Context context) : base(context)
		{
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
		}

		public View? MainView
		{
			get => _mainView;
			set
			{
				if (_mainView == value)
					return;

				if (_mainView != null)
					RemoveView(_mainView);
				
				_mainView = value;

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
					AddView(_mainView);
				}
			}
		}

		protected override void DispatchDraw(Canvas? canvas)
		{
			if (canvas != null && ClipShape != null)
			{
				var bounds = new RectangleF(0, 0, canvas.Width, canvas.Height);
				if (_lastPathSize != bounds.Size || _currentPath == null)
				{
					var path = ClipShape.CreatePath(bounds);
					_currentPath = path.AsAndroidPath();
					_lastPathSize = bounds.Size;
				}

				canvas.ClipPath(_currentPath);
			}

			base.DispatchDraw(canvas);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (_mainView == null)
				return;

			_mainView.Measure(widthMeasureSpec, heightMeasureSpec);
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension(_mainView.MeasuredWidth, _mainView.MeasuredHeight);
		}
	}
}