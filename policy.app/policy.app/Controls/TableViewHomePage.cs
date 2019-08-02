using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace policy.app.Controls
{
    public class TableViewHomePage : TableView
    {
        public static BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(TableViewHomePage));
        public Color SeparatorColor
        {
            get
            {
                return (Color)GetValue(SeparatorColorProperty);
            }
            set
            {
                SetValue(SeparatorColorProperty, value);
            }
        }
    }
}
