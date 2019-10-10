using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы рейтинга.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class RatingPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		private readonly App _app = App.Current;
		private IGopherService _service;
		private IGopher _selectedGopher;
		#endregion
		#endregion

		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		/// <param name="initData">Параметр модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (!_app.IsUserLoggedIn)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var token = repository.All()
								  .SingleOrDefault()
								  ?.Token;
			if (token != null)
			{
				_service = new GopherService(token);
				RefreshCommand.Execute(null);
			}
		}

		public Category SelectCategory
		{
			get;
			set;
		}
		
		public Category SelectedCategory
		{
			get;
			set;
		}

		public string SelectedSort
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает команду при выборе опроса.
		/// </summary>
		public Command<IGopher> EventSelected =>
			new Command<IGopher>(obj =>
			{
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});

		/// <summary>
		/// Возвращает или устанавливает выбранный опрос.
		/// </summary>
		public IGopher SelectedGopher
		{
			get => _selectedGopher;
			set
			{
				_selectedGopher = value;

				if (value != null)
				{
					EventSelected.Execute(value);
				}
			}
		}

		/// <summary>
		/// Возвращает или устанавливает сусликов в избранном.
		/// </summary>
		public ObservableCollection<IGopher> Gophers
		{
			get;
			set;
		}

		/// <summary>
		/// Загружает опросы.
		/// </summary>
		private async void LoadGophers()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .Single();
			Gophers = new ObservableCollection<IGopher>(await _service.Search("asc", Query, SelectCategory));
			
			repository.Update(user);
		}

		/// <summary>
		/// Возвращает или устанавливает перезагружается ли список избранных сусликов.
		/// </summary>
		public bool IsRefreshing
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает команду для обновления списка избранных. 
		/// </summary>
		public ICommand RefreshCommand =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				IsRefreshing = true;

				if (_service == null)
				{
					return;
				}

				LoadGophers();
				Categories = new ObservableCollection<Category>(await _service.GetCategories());

				IsRefreshing = false;
				tcs.SetResult(true);
			});

		public string Query
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает список категорий.
		/// </summary>
		public ObservableCollection<Category> Categories
		{
			get;
			set;
		}
	}
}
