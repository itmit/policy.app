using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using ImageCircle.Forms.Plugin.Droid;
using policy.app.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

[assembly: Dependency(typeof(AuthService))]

namespace policy.app.Droid
{
	[Activity(Label = "policy.app",
		Icon = "@mipmap/icon",
		Theme = "@style/MainTheme",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		#region Overrided
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			ImageCircleRenderer.Init();
			Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			
			CachedImageRenderer.Init(true);
			LoadApplication(new App());
		}
		#endregion
	}
}
