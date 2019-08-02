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

[assembly: Xamarin.Forms.Dependency(typeof(AuthService))]
namespace policy.app
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();

			Page loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			Page mainPage = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
			var mainContainer = new FreshNavigationContainer(mainPage, NavigationContainerNames.MainContainer);

			User user = Realm.GetInstance().All<User>()?.SingleOrDefault();

			if (IsUserLoggedIn | user != null)
			{
				MainPage = mainContainer;
				return;
			}

			MainPage = loginContainer;
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
