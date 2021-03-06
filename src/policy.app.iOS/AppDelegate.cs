﻿using Foundation;
using FFImageLoading.Forms.Platform;
using policy.app.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(AuthService))]

namespace policy.app.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// UserToken Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		#region Overrided
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Forms.Init();
			CachedImageRenderer.Init();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
		#endregion
	}
}
