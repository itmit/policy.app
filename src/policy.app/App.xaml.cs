using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using policy.app.RealmObjects;
using policy.app.Repositories;
using policy.app.Services;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using TabbedPage = Xamarin.Forms.TabbedPage;
using TabBar = Xamarin.Forms.PlatformConfiguration;

[assembly: Dependency(typeof(AuthService))]

namespace policy.app
{
	public partial class App : Application
	{
		#region .ctor
		public App()
		{
			Application.Current = this;

			Configuration = new MapperConfiguration(cfg =>
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

			Application.Current.On<TabBar.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

			var loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

			var repository = new UserRepository(RealmConfiguration);
			var user = repository.All();

			if (user.Any())
			{
				MainPage = InitMainTabbedPage();

				return;
			}

			MainPage = loginContainer;
		}
		#endregion

		#region Properties
		public MapperConfiguration Configuration
		{
			get;
		}

		public new static App Current => Application.Current as App;

		public RealmConfiguration RealmConfiguration
		{
			get
			{
				var configuration = RealmConfiguration.DefaultConfiguration;
				configuration.SchemaVersion = 7;
				return (RealmConfiguration) configuration;
			}
		}
		#endregion

		#region Public
		public TabbedPage InitMainTabbedPage()
		{
			var tabbedNavigation = new MainTabbedPage(NavigationContainerNames.MainContainer);

			var home = "ic_action_home.png";
			var star = "star_2.png";
			var search = "ic_action_search.png";
			var dehaze = "ic_action_dehaze.png";
			if (Device.iOS == Device.RuntimePlatform)
			{
				home = "baseline_home_white_24pt_1x.png";
				star = "baseline_star_white_24pt_1x.png";
				search = "baseline_search_white_24pt_1x.png";
				dehaze = "baseline_dehaze_white_24pt_1x.png";
			}

			tabbedNavigation.CurrentPage = tabbedNavigation.AddTab<CategoriesPageModel>("Категории", home);
			tabbedNavigation.MainPage = tabbedNavigation.CurrentPage;
			tabbedNavigation.AddTab<FavoritesPageModel>("Избранные", star);
			tabbedNavigation.AddTab<RatingPageModel>("Поиск", search);
			tabbedNavigation.AddTab<MenuPageModel>("Меню", dehaze);
			
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
		#endregion

		#region Overrided
		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			var culture = CultureInfo.GetCultureInfo("ru-RU");
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			if (MainPage is TabbedPage tabbedPage)
			{
				(tabbedPage.CurrentPage.BindingContext as BaseMainPageModel)?.LoadData();
			}
		}
		#endregion
	}
}
