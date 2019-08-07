using System;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using policy.app.Pages;
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

			var tabbedNavigation = new MainTabbedPage();
			tabbedNavigation.AddTab<HomePageModel>(null, "ic_action_home.png");
			tabbedNavigation.AddTab<SearchPageModel>(null, "ic_action_search.png");
			tabbedNavigation.AddTab<BackTabPageModel>(null, "ic_action_arrow_back.png");
			tabbedNavigation.AddTab<MenuPageModel>(null, "ic_action_dehaze.png");

			Page loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			User user = Realm.GetInstance().All<User>()?.SingleOrDefault();

			if (IsUserLoggedIn | user != null)
			{
				MainPage = tabbedNavigation;
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
