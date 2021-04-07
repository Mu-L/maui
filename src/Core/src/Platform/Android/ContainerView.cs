using System;
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
		APath? currentPath;
		Size lastPathSize;

		public ContainerView(Context? context) : base(context)
		{
			this.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
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
				var parent = _mainView?.Parent as ViewGroup;
				var index = parent?.IndexOfChild(_mainView);

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
					AddView(_mainView);
				}
			}
		}

		protected override void DispatchDraw(Canvas? canvas)
		{
			if (canvas != null && ClipShape != null)
			{
				var bounds = new RectangleF(0, 0, canvas.Width, canvas.Height);
				if (lastPathSize != bounds.Size || currentPath == null)
				{
					var path = ClipShape.CreatePath(bounds);
					currentPath = path.AsAndroidPath();
					lastPathSize = bounds.Size;
				}

				canvas.ClipPath(currentPath);
			}

			base.DispatchDraw(canvas);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (_mainView == null)
				return;

			_mainView.Measure(widthMeasureSpec, heightMeasureSpec);
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			this.SetMeasuredDimension(_mainView.MeasuredWidth, _mainView.MeasuredHeight);
		}
	}
}