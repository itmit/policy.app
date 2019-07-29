using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryAuthPage), typeof(EntryLogPageRenderer))]
namespace policy.app.Droid.Renderers
{
    public class EntryLogPageRenderer : EntryRenderer
    {
        public EntryLogPageRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Rgb(25,54,95));
            else
                Control.Background.SetColorFilter(Android.Graphics.Color.Rgb(25,54,95), PorterDuff.Mode.SrcAtop);
        }
    }
}