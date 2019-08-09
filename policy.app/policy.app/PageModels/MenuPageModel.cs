using System.Collections.ObjectModel;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы меню.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class MenuPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Определяет выбранный пункт в <see cref="Xamarin.Forms.ListView" />.
		/// </summary>
		private MenuItem _selectedItem;
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		public MenuPageModel() =>
			MenuCollection = new ObservableCollection<MenuItem>
			{
				new MenuItem
				{
					PageModelType = typeof(ProfilePageModel),
					Title = "Мой профиль"
				},
				new MenuItem
				{
					PageModelType = typeof(AboutPageModel),
					Title = "Избранное"
				},
				new MenuItem
				{
					PageModelType = typeof(AboutPageModel),
					Title = "Обратная связь"
				},
				new MenuItem
				{
					PageModelType = typeof(AboutPageModel),
					Title = "О нас"
				},
				new MenuItem
				{
					PageModelType = typeof(FirstPageModel),
					Title = "Приветствие"
				}
			};
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает список пунктов меню для домашней страницы.
		/// </summary>
		public ObservableCollection<MenuItem> MenuCollection
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает выбранный пункт меню.
		/// </summary>
		public MenuItem SelectedItem
		{
			get => _selectedItem;
			set
			{
				CoreMethods.PushPageModel(value.PageModelType, false);
				_selectedItem = value;
			}
		}
		#endregion
	}
}
