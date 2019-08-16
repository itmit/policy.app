using Android.Content;
using Android.Graphics;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PickerRating), typeof(PickerRatingRenderer))]
namespace policy.app.Droid.Renderers
{
    public class PickerRatingRenderer : PickerRenderer
    {
        public PickerRatingRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            this.Control.Background.SetColorFilter(Android.Graphics.Color.Rgb(244, 245, 246), PorterDuff.Mode.SrcAtop);
            Control?.SetPadding(20, 20, 20, 20);
            if (e.OldElement != null || e.NewElement != null)
            {
                var customPicker = e.NewElement as PickerRating;
                Control.TextSize = 25;
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
            }
        }
    }
}