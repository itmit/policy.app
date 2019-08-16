using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace policy.app.Controls
{
    public class PickerRating : Picker
    {
        public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(Int32), typeof(PickerRating), 24, BindingMode.TwoWay);

        public Int32 FontSize
        {
            set { SetValue(FontSizeProperty, value); }
            get { return (Int32)GetValue(FontSizeProperty); }
        }

        public static BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(PickerRating), "#19365f", BindingMode.TwoWay);

        public string PlaceholderColor
        {
            get { return (string)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }
    }
}
