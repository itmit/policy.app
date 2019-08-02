using Android.Content;
using Android.Graphics.Drawables;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TableViewHomePage), typeof(TableViewHomePageRenderer))]

namespace policy.app.Droid.Renderers
{
    public class TableViewHomePageRenderer : TableViewRenderer
    {
        public TableViewHomePageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;

            var listView = Control as Android.Widget.ListView;
            var coloredTableView = (TableViewHomePage)Element;
            listView.Divider = new ColorDrawable(coloredTableView.SeparatorColor.ToAndroid());
            listView.DividerHeight = 1;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "SeparatorColor")
            {
                var listView = Control as Android.Widget.ListView;
                var coloredTableView = (TableViewHomePage)Element;
                listView.Divider = new ColorDrawable(coloredTableView.SeparatorColor.ToAndroid());
            }
        }
    }
}