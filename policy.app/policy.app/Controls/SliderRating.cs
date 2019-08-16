using Xamarin.Forms;

namespace policy.app.Controls
{
    public class SliderRating : Slider
    {
        //Generate Custom Property
        public static readonly BindableProperty MaxColorProperty = BindableProperty.Create(nameof(MaxColor),
        typeof(Color), typeof(SliderRating), Color.Default);

        //Generate MaxColor Property
        public Color MaxColor
        {
            get { return (Color)GetValue(MaxColorProperty); }
            set { SetValue(MaxColorProperty, value); }
        }
        //Generate Min Property
        public static readonly BindableProperty MinColorProperty = BindableProperty.Create(nameof(MinColor),
            typeof(Color), typeof(SliderRating), Color.Default);

        public Color MinColor
        {
            get { return (Color)GetValue(MinColorProperty); }
            set { SetValue(MinColorProperty, value); }
        }
        //Generate Thumb Property
        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor),
            typeof(Color), typeof(SliderRating), Color.Default);

        public Color ThumbColor
        {
            get { return (Color)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }

        //Generate Thumb Image Property
        public static readonly BindableProperty ThumbImageProperty = BindableProperty.Create(nameof(ThumbImage),
              typeof(string), typeof(SliderRating), string.Empty);

        public string ThumbImage
        {
            get { return (string)GetValue(ThumbImageProperty); }
            set { SetValue(ThumbImageProperty, value); }
        }
    }
}
