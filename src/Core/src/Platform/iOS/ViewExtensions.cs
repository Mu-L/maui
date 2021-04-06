using System.Collections.Generic;
using System.Graphics.CoreGraphics;
using CoreAnimation;
using UIKit;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		const string ClipShapeLayer = "ClipShapeLayer";

		public static UIColor? GetBackgroundColor(this UIView view)
			=> view?.BackgroundColor;

		public static void UpdateIsEnabled(this UIView nativeView, IView view)
		{
			if (!(nativeView is UIControl uiControl))
				return;

			uiControl.Enabled = view.IsEnabled;
		}

		public static void UpdateBackgroundColor(this UIView nativeView, IView view)
		{
			if (nativeView == null)
				return;

			var color = view.BackgroundColor;

			if (!color.IsDefault)
				nativeView.BackgroundColor = color.ToNative();
		}
		public static void UpdateClipShape(this UIView nativeView, IView view)
		{
			var shouldUpdateClip = nativeView.ShouldUpdateClipShape(view);

			if (!shouldUpdateClip)
				return;

			var shape = view.ClipShape;
			var nativeShape = shape.ToNative(nativeView.Bounds.AsRectangleF(), 1.0f);

			var maskLayer = new CAShapeLayer
			{
				Name = ClipShapeLayer,
				Path = nativeShape
			};

			if (NativeVersion.IsAtLeast(11))
			{
				if (shape != null)
					nativeView.Layer.Mask = maskLayer;
				else
					nativeView.Layer.Mask = null;
			}
			else
			{
				if (shape != null)
				{
					var maskView = new UIView
					{
						Frame = nativeView.Frame,
						BackgroundColor = UIColor.Black
					};

					maskView.Layer.Mask = maskLayer;
					nativeView.MaskView = maskView;
				}
				else
					nativeView.MaskView = null;
			}
		}

		public static void UpdateAutomationId(this UIView nativeView, IView view) =>
			nativeView.AccessibilityIdentifier = view.AutomationId;

		public static T? FindDescendantView<T>(this UIView view) where T : UIView
		{
			var queue = new Queue<UIView>();
			queue.Enqueue(view);

			while (queue.Count > 0)
			{
				var descendantView = queue.Dequeue();

				if (descendantView is T result)
					return result;

				for (var i = 0; i < descendantView.Subviews?.Length; i++)
					queue.Enqueue(descendantView.Subviews[i]);
			}

			return null;
		}

		internal static bool ShouldUpdateClipShape(this UIView nativeView, IView view)
		{
			if (view == null || nativeView == null)
				return false;

			bool hasClipShapeLayer;

			if (NativeVersion.IsAtLeast(11))
				hasClipShapeLayer =
					nativeView.Layer != null &&
					nativeView.Layer.Mask != null &&
					nativeView.Layer.Mask?.Name == ClipShapeLayer;
			else
				hasClipShapeLayer =
					nativeView.MaskView != null &&
					nativeView.MaskView.Layer.Mask != null &&
					nativeView.MaskView.Layer.Mask?.Name == ClipShapeLayer;

			var clipShape = view.ClipShape;

			if (clipShape != null)
				return true;

			if (clipShape == null && hasClipShapeLayer)
				return true;

			return false;
		}
	}
}