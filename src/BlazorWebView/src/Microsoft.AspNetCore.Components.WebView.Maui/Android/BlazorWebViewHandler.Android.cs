﻿using Android.Util;
using Android.Webkit;
using static Android.Views.ViewGroup;
using AWebView = Android.Webkit.WebView;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using Path = System.IO.Path;

namespace Microsoft.AspNetCore.Components.WebView.Maui
{
	public partial class BlazorWebViewHandler : AbstractViewHandler<IBlazorWebView, AWebView>
	{
		public const string AssetBaseUrl = "file:///android_asset/";

		WebViewClient? _webViewClient;
		WebChromeClient? _webChromeClient;
		private AndroidWebKitWebViewManager? _webviewManager;
		internal AndroidWebKitWebViewManager? WebviewManager => _webviewManager;

		protected override AWebView CreateNativeView()
		{
			var aWebView = new AWebView(Context!)
			{
#pragma warning disable 618 // This can probably be replaced with LinearLayout(LayoutParams.MatchParent, LayoutParams.MatchParent); just need to test that theory
				LayoutParameters = new Android.Widget.AbsoluteLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, 0, 0)
#pragma warning restore 618
			};

			if (aWebView.Settings != null)
			{
				aWebView.Settings.JavaScriptEnabled = true;
				aWebView.Settings.DomStorageEnabled = true;
			}

			_webViewClient = GetWebViewClient();
			aWebView.SetWebViewClient(_webViewClient);

			_webChromeClient = GetWebChromeClient();
			aWebView.SetWebChromeClient(_webChromeClient);

			return aWebView;
		}

		protected override void DisconnectHandler(AWebView nativeView)
		{
			nativeView.StopLoading();

			_webViewClient?.Dispose();
			_webChromeClient?.Dispose();
		}

		private bool RequiredStartupPropertiesSet =>
			//_webview != null &&
			HostPage != null &&
			Services != null;

		public string? HostPage { get; private set; }
		public ObservableCollection<RootComponent>? RootComponents { get; private set; }
		public new IServiceProvider? Services { get; private set; }

		private void StartWebViewCoreIfPossible()
		{
			if (!RequiredStartupPropertiesSet ||
				false)//_webviewManager != null)
			{
				return;
			}
			if (TypedNativeView == null)
			{
				throw new InvalidOperationException($"Can't start {nameof(BlazorWebView)} without native web view instance.");
			}

			var resourceAssembly = RootComponents?[0]?.ComponentType?.Assembly;
			if (resourceAssembly == null)
			{
				throw new InvalidOperationException($"Can't start {nameof(BlazorWebView)} without a component type assembly.");
			}

			// We assume the host page is always in the root of the content directory, because it's
			// unclear there's any other use case. We can add more options later if so.
			var contentRootDir = Path.GetDirectoryName(HostPage) ?? string.Empty;
			var hostPageRelativePath = Path.GetRelativePath(contentRootDir, HostPage!);
			var fileProvider = new ManifestEmbeddedFileProvider(resourceAssembly, root: contentRootDir);

			_webviewManager = new AndroidWebKitWebViewManager(this, TypedNativeView, Services!, MauiDispatcher.Instance, fileProvider, hostPageRelativePath);
			if (RootComponents != null)
			{
				foreach (var rootComponent in RootComponents)
				{
					// Since the page isn't loaded yet, this will always complete synchronously
					_ = rootComponent.AddToWebViewManagerAsync(_webviewManager);
				}
			}

			_webviewManager.Navigate("/");
		}

		public static void MapHostPage(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			handler.HostPage = webView.HostPage;
			handler.StartWebViewCoreIfPossible();
		}

		public static void MapRootComponents(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			handler.RootComponents = webView.RootComponents;
			handler.StartWebViewCoreIfPossible();
		}

		public static void MapServices(BlazorWebViewHandler handler, IBlazorWebView webView)
		{
			handler.Services = webView.Services;
			handler.StartWebViewCoreIfPossible();
		}

		public void LoadHtml(string? html, string? baseUrl)
		{
			TypedNativeView?.LoadDataWithBaseURL(baseUrl ?? AssetBaseUrl, html ?? string.Empty, "text/html", "UTF-8", null);
		}

		public void LoadUrl(string? url)
		{
			TypedNativeView?.LoadUrl(url ?? string.Empty);
		}

		protected virtual WebViewClient GetWebViewClient() =>
			new WebKitWebViewClient(this);

		protected virtual WebChromeClient GetWebChromeClient() =>
			new WebChromeClient();
	}
}
