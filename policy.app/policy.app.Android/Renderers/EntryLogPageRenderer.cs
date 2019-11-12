using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(EntryAuthPage), typeof(EntryLogPageRenderer))]

namespace policy.app.Droid.Renderers
{
	public class EntryLogPageRenderer : EntryRenderer
	{
		#region .ctor
		public EntryLogPageRenderer(Context context)
			: base(context)
		{
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null || e.NewElement == null)
			{
				return;
			}

			Control.BackgroundTintList = ColorStateList.ValueOf(Color.Rgb(25, 54, 95));
			Control.Background.SetColorFilter(Color.Rgb(25, 54, 95), PorterDuff.Mode.SrcAtop);
		}
		#endregion
	}
}
