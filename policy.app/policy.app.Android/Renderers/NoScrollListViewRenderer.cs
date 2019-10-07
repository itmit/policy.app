

using Android.Content;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoScrollListView), typeof(NoScrollListViewRenderer))]
namespace policy.app.Droid.Renderers
{
	public class NoScrollListViewRenderer : ListViewRenderer
	{
		public NoScrollListViewRenderer(Context context)
			: base(context)
		{}

		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				return;
			}

			if (Control != null)
			{
				Control.NestedScrollingEnabled = false;
				Control.VerticalScrollBarEnabled = false;
				Control.HorizontalScrollBarEnabled = false;
			}

			NestedScrollingEnabled = false;
			HorizontalScrollBarEnabled = false;
			VerticalScrollBarEnabled = false;
		}
	}
}
