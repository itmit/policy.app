using Android.Widget;
using policy.app.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ListViewEffect), "ListViewEffect")]
namespace policy.app.Droid.Renderers
{
    public class ListViewEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var listView = (Android.Widget.ListView)Control;

            listView.ChoiceMode = ChoiceMode.None;
        }

        protected override void OnDetached()
        {

        }
    }
}