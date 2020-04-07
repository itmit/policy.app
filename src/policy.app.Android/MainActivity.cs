using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Util;
using FFImageLoading.Forms.Platform;
using Firebase.Messaging;
using ImageCircle.Forms.Plugin.Droid;
using policy.app.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

[assembly: Dependency(typeof(AuthService))]

namespace policy.app.Droid
{
	[Activity(Label = "Политика",
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

		internal static readonly string ChannelId = "my_notification_channel";
		internal static readonly int NotificationId = 100;

		private const string Tag = "GardenerMainActivity";

		private void CreateNotificationChannel()
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.O)
			{
				// Notification channels are new in API 26 (and not a part of the
				// support library). There is no need to create a notification
				// channel on older versions of Android.
				return;
			}

			var channel = new NotificationChannel(ChannelId,
												  "FCM Notifications",
												  NotificationImportance.Default)
			{

				Description = "Firebase Cloud Messages appear in this channel"
			};

			var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
			notificationManager.CreateNotificationChannel(channel);
		}
		 
		protected override void OnCreate(Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			CreateNotificationChannel();
			var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			Log.Info(Tag, $"IsGooglePlayServicesAvailable code is {resultCode}");
			FirebaseMessaging.Instance.SubscribeToTopic("all");

			ImageCircleRenderer.Init();
			Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			
			CachedImageRenderer.Init(true);
			LoadApplication(new App());
		}
		#endregion
	}
}
