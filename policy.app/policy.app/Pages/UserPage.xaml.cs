using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Browser.OpenAsync("https://ru.wikipedia.org/wiki/", BrowserLaunchMode.SystemPreferred);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var _numL = int.Parse(Like.Text) + 1;

            Like.Text = Convert.ToString(_numL);
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            var _numL = int.Parse(Dislike.Text) + 1;

            Dislike.Text = Convert.ToString(_numL);
        }
    }
}