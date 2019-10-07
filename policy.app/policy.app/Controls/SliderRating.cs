using Xamarin.Forms;

namespace policy.app.Controls
{
	public class SliderRating : Slider
	{
		#region Data
		#region Static
		//Generate Custom Property
		public static readonly BindableProperty MaxColorProperty = BindableProperty.Create(nameof(MaxColor), typeof(Color), typeof(SliderRating), Color.Default);

		//Generate Min Property
		public static readonly BindableProperty MinColorProperty = BindableProperty.Create(nameof(MinColor), typeof(Color), typeof(SliderRating), Color.Default);
		#endregion
		#endregion

		#region Properties
		//Generate MaxColor Property
		public Color MaxColor
		{
			get => (Color) GetValue(MaxColorProperty);
			set => SetValue(MaxColorProperty, value);
		}

		public Color MinColor
		{
			get => (Color) GetValue(MinColorProperty);
			set => SetValue(MinColorProperty, value);
		}
		#endregion
	}
}
