using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace policy.app.Controls
{
    public class TableViewHomePage : TableView
    {
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(TableViewHomePage));

		public Color SeparatorColor
        {
            get => (Color)GetValue(SeparatorColorProperty);
			set => SetValue(SeparatorColorProperty, value);
		}
    }
}
