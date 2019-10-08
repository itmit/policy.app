using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using policy.app.PageModels;
using policy.app.Pages;
using Xamarin.Forms;

namespace policy.app
{
	/// <summary>
	/// Представляет главную страницу меню, отвечающую за навигацию.
	/// </summary>
	public class MainTabbedPage : TabbedPage, IFreshNavigationService
	{
		#region Data
		#region Fields
		/// <summary>
		/// Список страниц отображаемых в меню.
		/// </summary>
		private readonly List<Page> _tabs = new List<Page>();
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="MainTabbedPage" />, регистрируя сервис навигации.
		/// </summary>
		public MainTabbedPage(string navigationServiceName)
		{
			NavigationServiceName = navigationServiceName;
			RegisterNavigation();
		}
		#endregion

		#region Overridable
		/// <summary>
		/// Добавляет страницу в меню.
		/// </summary>
		/// <typeparam name="T">Модель представления страницы.</typeparam>
		/// <param name="title">Заголовок страницы.</param>
		/// <param name="icon">Иконка страницы.</param>
		/// <param name="data">Данные передаваемые в меню.</param>
		/// <returns>Созданная страница.</returns>
		public virtual Page AddTab<T>(string title, string icon, object data = null) where T : FreshBasePageModel
		{
			var page = FreshPageModelResolver.ResolvePageModel<T>(data);
			page.GetModel()
				.CurrentNavigationServiceName = NavigationServiceName;
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

		/// <summary>
		/// Создает страницу навигации.
		/// </summary>
		/// <param name="page">Изначальная страница.</param>
		/// <returns>Страница навигации.</returns>
		protected virtual Page CreateContainerPage(Page page) => new NavigationPage(page);
		#endregion

		#region IFreshNavigationService members
		/// <summary>
		/// Возвращает название сервиса навигации.
		/// </summary>
		public string NavigationServiceName
		{
			get;
		}

		/// <summary>
		/// Уведомляет потомков об остановке. 
		/// </summary>
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

		/// <summary>
		/// Покидает текущую страницу.
		/// </summary>
		/// <param name="modal">Покинуть ли страницу модально?</param>
		/// <param name="animate">Покинуть ли страницу с анимацией?</param>
		/// <returns>Возвращает операцию.</returns>
		public Task PopPage(bool modal = false, bool animate = true)
		{
			if (modal)
			{
				return CurrentPage.Navigation.PopModalAsync(animate);
			}

			return CurrentPage.Navigation.PopAsync(animate);
		}

		/// <summary>
		/// Возвращает к корневой странице.
		/// </summary>
		/// <param name="animate">Вернуть ли с анимацией?</param>
		/// <returns>Возвращает операцию.</returns>
		public Task PopToRoot(bool animate = true) => CurrentPage.Navigation.PopToRootAsync(animate);

		/// <summary>
		/// Открывает страницу.
		/// </summary>
		/// <param name="page">Открываемая страница.</param>
		/// <param name="model">Контекст привязки страницы.</param>
		/// <param name="modal">Открыть ли страницу модально?</param>
		/// <param name="animate">Открыть ли страницу с анимацией?</param>
		/// <returns>Возвращает операцию.</returns>
		public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
		{
			if (modal)
			{
				return CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
			}

			return CurrentPage.Navigation.PushAsync(page);
		}

		/// <summary>
		/// Переключает выбранную корневую модель страницы.
		/// </summary>
		/// <typeparam name="T">Тип модели представления страницы</typeparam>
		/// <returns>Возвращает операцию.</returns>
		public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
		{
			var page = _tabs.FindIndex(o => o.GetModel()
											 .GetType()
											 .FullName ==
											typeof(T).FullName);

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
		#endregion

		#region Private
		/// <summary>
		/// Создает страницу навигации, с учетом типа страницы.
		/// </summary>
		/// <param name="page">Изначальная страница.</param>
		/// <returns>Страница навигации.</returns>
		private Page CreateContainerPageSafe(Page page)
		{
			if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
			{
				return page;
			}

			return CreateContainerPage(page);
		}

		/// <summary>
		/// Регистрирует сервис навигации.
		/// </summary>
		private void RegisterNavigation()
		{
			FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
		}
		#endregion
	}
}
