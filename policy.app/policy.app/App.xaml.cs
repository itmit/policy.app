using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using policy.app.Pages;
using policy.app.RealmObjects;
using policy.app.Repositories;
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
		private readonly MapperConfiguration _configuration;

		public new static App Current => Application.Current as App;

		public App()
		{
			_configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddCollectionMappers();

				cfg.CreateMap<User, UserRealmObject>()
				   .ForMember(q => q.FavoriteGophers, opt => opt.MapFrom(q => q.FavoriteGophers));

				cfg.CreateMap<Gopher, GopherRealmObject>();
				cfg.CreateMap<UserToken, TokenRealmObject>();
				cfg.CreateMap<Category, CategoryRealmObject>();

				cfg.CreateMap<UserRealmObject, User>()
				   .ForMember(q => q.FavoriteGophers, opt => opt.MapFrom(q => q.FavoriteGophers));


				cfg.CreateMap<GopherRealmObject, Gopher>();
				cfg.CreateMap<TokenRealmObject, UserToken>();
				cfg.CreateMap<CategoryRealmObject, Category>();
			});

			InitializeComponent();

			Page loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			var repository = new UserRepository(RealmConfiguration);
			var user = repository.All();
			if (IsUserLoggedIn | user.Any())
			{
				IsUserLoggedIn = true;
				MainPage = InitMainTabbedPage();

                return;
			}

			MainPage = loginContainer;
		}

		public MapperConfiguration Configuration => _configuration;

		public RealmConfiguration RealmConfiguration
		{
			get
			{
				var configuration = RealmConfiguration.DefaultConfiguration;
				configuration.SchemaVersion = 6;
				return (RealmConfiguration)configuration;
			}
		}

		public TabbedPage InitMainTabbedPage()
		{
			var tabbedNavigation = new MainTabbedPage(NavigationContainerNames.MainContainer);

            tabbedNavigation.CurrentPage = tabbedNavigation.AddTab<CategoriesPageModel>(null, "ic_action_home.png");
            tabbedNavigation.AddTab<FavoritesPageModel>(null, "star_2.png");
            tabbedNavigation.AddTab<RatingPageModel>(null, "ic_action_search.png");
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
