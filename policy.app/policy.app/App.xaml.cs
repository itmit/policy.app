using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using policy.app.Services;
using policy.app.ViewModels.PageModel;
using policy.app.Views;

namespace policy.app
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AuthPage();


			/*
			var loginPage = FreshPageModelResolver.ResolvePageModel<TermsOfUsePageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			var mainPage = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
			var mainContainer = new FreshNavigationContainer(mainPage, NavigationContainerNames.MainContainer);
			*/
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
