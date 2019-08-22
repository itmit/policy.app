using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using Xamarin.Forms;
using System.Collections.Generic;
using policy.app.Pages;

namespace policy.app
{
	public class MainTabbedPage : TabbedPage, IFreshNavigationService
	{
		private readonly List<Page> _tabs = new List<Page>();
		private NavigationPage _previewsPage;

		public IEnumerable TabbedPages => _tabs;

		public MainTabbedPage()
			: this(Constants.DefaultNavigationServiceName)
		{ 
			CurrentPageChanged += CurrentPageHasChanged;
		}

		private void CurrentPageHasChanged(object sender, EventArgs e)
		{
			if (sender is Xamarin.Forms.TabbedPage tabbedPage)
			{
				if (tabbedPage.CurrentPage is NavigationPage navigationPage)
				{
					_previewsPage = navigationPage;
					return;
				}

				if (tabbedPage.CurrentPage is BackTabPage)
				{
					tabbedPage.CurrentPage = _previewsPage;
					if (_previewsPage.Navigation.NavigationStack.Count > 0)
					{
						PopPage();
					}
				}
			}
		}

		public MainTabbedPage(string navigationServiceName)
		{
			CurrentPageChanged += CurrentPageHasChanged;
			NavigationServiceName = navigationServiceName;
			RegisterNavigation();
		}

		protected void RegisterNavigation()
		{
			FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
		}

		public virtual Page AddTab<T>(string title, string icon, object data = null) where T : FreshBasePageModel
		{
			var page = FreshPageModelResolver.ResolvePageModel<T>(data);
			page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
			_tabs.Add(page);
			var navigationContainer = CreateContainerPageSafe(page);
			navigationContainer.Title = title;
			if (!string.IsNullOrWhiteSpace(icon))
			{
				navigationContainer.IconImageSource = icon;
			}

			Children.Add(navigationContainer);
			return navigationContainer;
		}

		internal Page CreateContainerPageSafe(Page page)
		{
			if (page is NavigationPage || page is MasterDetailPage || page is Xamarin.Forms.TabbedPage)
			{
				return page;
			}

			return CreateContainerPage(page);
		}

		protected virtual Page CreateContainerPage(Page page)
		{
			if (page is BackTabPage)
			{
				return page;
			}

			return new NavigationPage(page);
		}

		public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
		{
			if (modal)
			{
				return CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
			}

			return CurrentPage.Navigation.PushAsync(page);
		}

		public Task PopPage(bool modal = false, bool animate = true)
		{
			if (modal)
			{
				return CurrentPage.Navigation.PopModalAsync(animate);
			}

			return CurrentPage.Navigation.PopAsync(animate);
		}

		public Task PopToRoot(bool animate = true) => CurrentPage.Navigation.PopToRootAsync(animate);

		public string NavigationServiceName { get; }

		public void NotifyChildrenPageWasPopped()
		{
			foreach (var page in Children)
			{
				if (page is NavigationPage navigationPage)
				{
					navigationPage.NotifyAllChildrenPopped();
				}
			}
		}

		public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
		{
			var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

			if (page > -1)
			{
				CurrentPage = Children[page];
				var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
				if (topOfStack != null)
				{
					return Task.FromResult(topOfStack.GetModel());
				}
			}
			return null;
		}

	}
}
