using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace policy.app.Controls
{
    public class PickerRating : Picker
    {
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(PickerRating), "#19365f", BindingMode.TwoWay);

        public string PlaceholderColor
        {
            get => (string)GetValue(PlaceholderColorProperty);
			set => SetValue(PlaceholderColorProperty, value);
		}
    }
}
