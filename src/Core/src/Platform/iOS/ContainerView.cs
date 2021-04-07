using System;
using System.Graphics;
using System.Graphics.CoreGraphics;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Microsoft.Maui
{
	public partial class ContainerView : UIView
	{
		UIView? _mainView;
		Size lastMaskSize;

		public ContainerView()
		{
			AutosizesSubviews = true;
			lastMaskSize = Size.Zero;
		}

		public UIView? MainView
		{
			get => _mainView;
			set
			{
				if (_mainView == value)
					return;

				if (_mainView != null)
					_mainView.RemoveFromSuperview();
				
				_mainView = value;

				if (_mainView == null)
					return;

				this.Frame = _mainView.Frame;
				var oldParent = _mainView.Superview;

				if (oldParent != null)
					oldParent.InsertSubviewAbove(this, _mainView);

				_mainView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_mainView.Frame = Bounds;

				AddSubview(_mainView);
			}
		}

		public override void SizeToFit()
		{
			if (MainView == null)
				return;

			MainView.SizeToFit();
			this.Bounds = MainView.Bounds;

			base.SizeToFit();
		}

		CAShapeLayer? Mask
		{
			get => MainView?.Layer.Mask as CAShapeLayer;
			set => MainView.Layer.Mask = value;
		}

		partial void ClipShapeChanged()
		{
			lastMaskSize = Size.Zero;

			if (Frame == CGRect.Empty)
				return;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			SetClipShape();
		}

		void SetClipShape()
		{
			var mask = Mask;

			if (mask == null && ClipShape == null)
				return;

			mask ??= Mask = new CAShapeLayer();
			var frame = Frame;
			var bounds = new RectangleF(0, 0, frame.Width, frame.Height);

			if (bounds.Size == lastMaskSize)
				return;

			lastMaskSize = bounds.Size;
			var path = _clipShape?.CreatePath(bounds);
			mask.Path = path?.AsCGPath();
		}
	}
}