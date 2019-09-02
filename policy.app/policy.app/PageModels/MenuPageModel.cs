﻿using System.Collections.ObjectModel;
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
                    ImageSource = "menu_5.png",
                    PageModelType = typeof(ProfilePageModel),
					Title = "Мой профиль"
				},
                new MenuItem
                {
                    ImageSource = "menu_6.png",
                    PageModelType = typeof(PollPageModel),
                    Title = "Опросы"
                },
                new MenuItem
				{
                    ImageSource = "menu_3.png",
                    PageModelType = typeof(WriteToUsPageModel),
					Title = "Напишите нам"
				},
				new MenuItem
				{
                    ImageSource = "menu_7.png",
                    PageModelType = typeof(AboutPageModel),
					Title = "О приложении"
				},
                new MenuItem
                {
                    ImageSource = "menu_4.png",
                    PageModelType = typeof(AboutPageModel),
                    Title = "Выход"
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
        /*
		public MenuItem SelectedItem
		{
			get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (value != null)
                {
                    CoreMethods.PushPageModel(value.PageModelType, false);
                }

            }
        }*/
        #endregion

        public MenuItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                if (value != null)
                    EventSelected.Execute(value);
            }
        }

        public Xamarin.Forms.Command<MenuItem> EventSelected
        {
            get
            {
                return new Xamarin.Forms.Command<MenuItem>( obj => {
                    if(obj is MenuItem menuItem)
                    CoreMethods.PushPageModel(menuItem.PageModelType, false);
                });
            }
        }

    }
}
