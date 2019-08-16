﻿using System.Collections.ObjectModel;
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
		/// <summary>
		/// Определяет выбранный пункт в <see cref="Xamarin.Forms.ListView"/>.
		/// </summary>
		private MenuItem _selectedItem;

		#region .ctor
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		public HomePageModel()
		{
			MenuCollection = new ObservableCollection<MenuItem>
			{
                new MenuItem
                {
                    PageModelType = typeof(ProfilePageModel),
                    Title = "Мой профиль"
                },
                new MenuItem
                {
                    PageModelType = typeof(RatingPageModel),
                    Title = "Рейтинг"
                },
                new MenuItem
				{
					PageModelType = typeof(RatingPageModel),
					Title = "Политика"
				},
				new MenuItem
				{
					PageModelType = typeof(RatingPageModel),
					Title = "Музыка"
				},
				new MenuItem
				{
					PageModelType = typeof(RatingPageModel),
					Title = "Кино"
				},
				new MenuItem
				{
					PageModelType = typeof(RatingPageModel),
					Title = "Телевидение"
				}
			};
		}
		#endregion

		#region Properties
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
