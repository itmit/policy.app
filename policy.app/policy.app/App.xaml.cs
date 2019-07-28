using System;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using policy.app.Services;
using policy.app.Views;
using Realms;

namespace policy.app
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new MainPage();

			var loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			var mainPage = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
			var mainContainer = new FreshNavigationContainer(mainPage, NavigationContainerNames.MainContainer);

			var realm = Realm.GetInstance();
			var user = realm.All<User>();
			var userIsFound = user?.Count() > 0;

			if (IsUserLoggedIn | userIsFound)
			{
				MainPage = mainContainer;
			}
			else
			{
				MainPage = loginContainer;
			}
		}

		public static bool IsUserLoggedIn
		{
			get;
			set;
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
