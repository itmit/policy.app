using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(SliderRating), typeof(SIiderRenderer))]

namespace policy.app.Droid.Renderers
{
	public class SIiderRenderer : SliderRenderer
	{
		#region Data
		#region Fields
		private SliderRating view;
		#endregion
		#endregion

		#region .ctor
		public SIiderRenderer(Context context)
			: base(context)
		{
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || e.NewElement == null)
			{
				return;
			}

			view = (SliderRating) Element;
			if (!string.IsNullOrEmpty(view.ThumbImage))
			{
				// Set Thumb Icon  
				Control.SetThumb(Resources.GetDrawable(view.ThumbImage));
			}
			else if (view.ThumbColor != Color.Default || view.MaxColor != Color.Default || view.MinColor != Color.Default)
			{
				Control.Thumb.SetColorFilter(view.ThumbColor.ToAndroid(), PorterDuff.Mode.SrcIn);
			}

			Control.ProgressTintList = ColorStateList.ValueOf(view.MinColor.ToAndroid());
			Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
			//this is for Maximum Slider line Color  
			Control.ProgressBackgroundTintList = ColorStateList.ValueOf(view.MaxColor.ToAndroid());
			Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
			{
				if (Control == null)
				{
					return;
				}

				var ctrl = Control;
				var thumb = ctrl.Thumb;
				var thumbTop = ctrl.Height / 2 - thumb.IntrinsicHeight / 2;
				thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
			}
		}
		#endregion
	}
}
