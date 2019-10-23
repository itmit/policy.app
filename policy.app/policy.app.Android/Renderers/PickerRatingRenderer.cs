using Android.Content;
using Android.Graphics;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(PickerRating), typeof(PickerRatingRenderer))]

namespace policy.app.Droid.Renderers
{
	public class PickerRatingRenderer : PickerRenderer
	{
		#region .ctor
		public PickerRatingRenderer(Context context)
			: base(context)
		{
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);
			Control.Background.SetColorFilter(Color.Rgb(244, 245, 246), PorterDuff.Mode.SrcAtop);
			Control?.SetPadding(20, 20, 20, 20);
			if (e.OldElement != null || e.NewElement != null)
			{
				var customPicker = e.NewElement as PickerRating;
				Control.TextSize = 16;
				Control.SetHintTextColor(Color.ParseColor(customPicker.PlaceholderColor));
			}
		}
		#endregion
	}
}
