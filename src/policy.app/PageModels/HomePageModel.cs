using System.Collections.ObjectModel;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для домашней страницы.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class HomePageModel : FreshBasePageModel
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
		public HomePageModel() =>
			MenuCollection = new ObservableCollection<MenuItem>
			{
				new MenuItem("Мой профиль", typeof(ProfilePageModel)),
				new MenuItem("Рейтинг", typeof(RatingPageModel)),
				new MenuItem("Поиск", typeof(SearchPageModel))
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
