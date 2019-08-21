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
using TabBar = Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using TabbedPage = Xamarin.Forms.TabbedPage;

[assembly: Xamarin.Forms.Dependency(typeof(AuthService))]
namespace policy.app
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			Page loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			User user = Realm.All<User>()?.SingleOrDefault();

			if (IsUserLoggedIn | user != null)
			{
				IsUserLoggedIn = true;
				MainPage = InitMainTabbedPage();
				//MainPage = new TabbedPage1();
				return;
			}

			MainPage = loginContainer;
		}

		public Realm Realm
		{
			get
			{
				var configuration = RealmConfiguration.DefaultConfiguration;
				configuration.SchemaVersion = 2;
				return Realm.GetInstance(configuration);
			}
		}

		public TabbedPage InitMainTabbedPage()
		{
			var tabbedNavigation = new MainTabbedPage();
			tabbedNavigation.AddTab<HomePageModel>(null, "ic_action_home.png");
            tabbedNavigation.AddTab<FavouritesPageModel>(null, "baseline_grade_white_18dp");
            tabbedNavigation.AddTab<SearchPageModel>(null, "ic_action_search.png");
			//tabbedNavigation.AddTab<BackTabPageModel>(null, "ic_action_arrow_back.png");
			tabbedNavigation.AddTab<MenuPageModel>(null, "ic_action_dehaze.png");

			tabbedNavigation.Effects.Add(new NoShiftEffect());
			tabbedNavigation.On<TabBar.Android>()
				.SetToolbarPlacement(ToolbarPlacement.Bottom);
			tabbedNavigation.On<TabBar.Android>()
							.SetIsSwipePagingEnabled(false);
			tabbedNavigation.BarBackgroundColor = Color.FromHex("#228bcc");
			tabbedNavigation.SelectedTabColor = Color.Black;
			tabbedNavigation.UnselectedTabColor = Color.White;

			return tabbedNavigation;
		}

		public bool IsUserLoggedIn
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
