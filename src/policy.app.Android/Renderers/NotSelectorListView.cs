using Android.Content;
using policy.app.Controls;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListViewNoSelect), typeof(NotSelectorListView))]

namespace policy.app.Droid.Renderers
{
	public class NotSelectorListView : ListViewRenderer
	{
		#region .ctor
		public NotSelectorListView(Context context)
			: base(context)
		{
		}
		#endregion

		#region Overrided
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);
			Control.SetSelector(Resource.Drawable.no_selector);
		}
		#endregion
	}
}
