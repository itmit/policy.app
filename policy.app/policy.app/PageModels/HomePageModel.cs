using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;
using Realms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для домашней страницы.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class HomePageModel : FreshBasePageModel
	{
		#region .ctor
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		public HomePageModel()
		{
			MenuCollection = new ObservableCollection<MenuItem>
			{
				//new MenuItem
			};
		}
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает выбранный пункт меню.
		/// </summary>
		public string SelectedItem
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает список пунктов меню для домашней страницы.
		/// </summary>
		public ObservableCollection<MenuItem> MenuCollection
		{
			get;
			set;
		}
		#endregion
	}
}
