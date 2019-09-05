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
                new MenuItem("Мой профиль", typeof(ProfilePageModel))
				{
                    ImageSource = "menu_1_def.png"
				},
                new MenuItem("Напишите нам", typeof(WriteToUsPageModel))
				{
                    ImageSource = "menu_3_def.png"
				},
				new MenuItem("Напишите нам", typeof(AboutPageModel))
				{
                    ImageSource = "menu_4_def.png"
				},
                new MenuItem("Выход", typeof(AboutPageModel))
                {
                    ImageSource = "menu_5_def.png"
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
		#endregion

		/// <summary>
		/// Возвращает или устанавливает выбранный пункт меню.
		/// </summary> 
		public MenuItem SelectedItem
        {
            get =>_selectedItem;
            set
            {
                _selectedItem = value;

                if (value != null)
				{
					EventSelected.Execute(value);
				}
			}
        }

        public Xamarin.Forms.Command<MenuItem> EventSelected =>
			new Xamarin.Forms.Command<MenuItem>( obj => {
				if(obj is MenuItem menuItem)
				{
					CoreMethods.PushPageModel(menuItem.PageModelType, false);
				}
			});
	}
}
