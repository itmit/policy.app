using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllquestionsPage : ContentPage
    {
        public AllquestionsPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SurveyPage());
        }
    }
}