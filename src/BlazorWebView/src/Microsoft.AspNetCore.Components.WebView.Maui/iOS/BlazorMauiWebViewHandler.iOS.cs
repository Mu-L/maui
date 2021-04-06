﻿using System;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using UIKit;
using WebKit;
using RectangleF = CoreGraphics.CGRect;

namespace Microsoft.AspNetCore.Components.WebView.Maui
{
    public partial class BlazorWebViewHandler : AbstractViewHandler<IBlazorWebView, WKWebView>
    {
		static WKProcessPool? SharedPool;

		protected override WKWebView CreateNativeView()
		{
			return new WKWebView(RectangleF.Empty, CreateConfiguration())
			{
				BackgroundColor = UIColor.Clear,
				AutosizesSubviews = true
			};
		}

		// https://developer.apple.com/forums/thread/99674
		// WKWebView and making sure cookies synchronize is really quirky
		// The main workaround I've found for ensuring that cookies synchronize 
		// is to share the Process Pool between all WkWebView instances.
		// It also has to be shared at the point you call init
		static WKWebViewConfiguration CreateConfiguration()
		{
			var config = new WKWebViewConfiguration();

			if (SharedPool == null)
				SharedPool = config.ProcessPool;
			else
				config.ProcessPool = SharedPool;

			return config;
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SetDesiredSize(widthConstraint, heightConstraint);

			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public void LoadHtml(string? html, string? baseUrl)
		{
			if (html != null)
				TypedNativeView?.LoadHtmlString(html, baseUrl == null ? new NSUrl(NSBundle.MainBundle.BundlePath, true) : new NSUrl(baseUrl, true));
		}

		public void LoadUrl(string? url)
		{
			var uri = new Uri(url ?? string.Empty);
			var safeHostUri = new Uri($"{uri.Scheme}://{uri.Authority}", UriKind.Absolute);
			var safeRelativeUri = new Uri($"{uri.PathAndQuery}{uri.Fragment}", UriKind.Relative);
			NSUrlRequest request = new NSUrlRequest(new Uri(safeHostUri, safeRelativeUri));

			TypedNativeView?.LoadRequest(request);
		}

		void SetDesiredSize(double width, double height)
		{
			if (TypedNativeView != null)
			{
				var x = TypedNativeView.Frame.X;
				var y = TypedNativeView.Frame.Y;

				TypedNativeView.Frame = new RectangleF(x, y, width, height);
			}
		}













		public static void MapHostPage(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			throw new NotImplementedException();
		}

		public static void MapRootComponents(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			throw new NotImplementedException();
		}

		public static void MapServices(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			throw new NotImplementedException();
		}
	}
}
