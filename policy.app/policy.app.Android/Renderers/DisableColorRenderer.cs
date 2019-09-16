using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace policy.app.Droid.Renderers
{
    public class DisableColorRenderer : ListViewRenderer
    {
        public DisableColorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
            {
                return;
            }

            if (e.PropertyName == "SelectedItem")
            {
                //Control.SetBackgroundColor(Control);
            }
        }

    }
}