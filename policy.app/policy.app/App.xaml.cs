﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
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

                return;
			}

			MainPage = loginContainer;
		}

		public Realm Realm
		{
			get
			{
				var configuration = RealmConfiguration.DefaultConfiguration;
				configuration.SchemaVersion = 4;
				return Realm.GetInstance(configuration);
			}
		}

		public TabbedPage InitMainTabbedPage()
		{
			var tabbedNavigation = new MainTabbedPage(NavigationContainerNames.MainContainer);

			tabbedNavigation.AddTab<CategoriesPageModel>(null, "ic_action_home.png");
            tabbedNavigation.AddTab<FavouritesPageModel>(null, "star_2.png");
            tabbedNavigation.AddTab<RatingPageModel>(null, "ic_action_search.png");
            tabbedNavigation.CurrentPage = tabbedNavigation.AddTab<MenuPageModel>(null, "ic_action_dehaze.png");

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
			var culture = CultureInfo.GetCultureInfo("ru-RU");
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
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
