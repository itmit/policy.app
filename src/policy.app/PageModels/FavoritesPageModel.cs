using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы списка избранных сусликов.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class FavoritesPageModel : BaseMainPageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;

		private UserRepository _repository;

		/// <summary>
		/// Выбранный суслик.
		/// </summary>
		private IGopher _selectedGopher;

		/// <summary>
		/// Сервис для загрузки сусликов.
		/// </summary>
		private IGopherService _service;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает сусликов в избранном.
		/// </summary>
		public ObservableCollection<IGopher> Gophers
		{
			get;
			set;
		}

		/// <summary>
		/// Представляет метод обновления списка избранных.
		/// </summary>
		public delegate void UpdateFavoritesEventHandler();

		/// <summary>
		/// Происходит после обновлений списка избранных.
		/// </summary>
		public static event UpdateFavoritesEventHandler UpdateFavorites;

		/// <summary>
		/// Провоцирует событие <see cref="UpdateFavorites"/>.
		/// </summary>
		public static void InvokeUpdateFavorites()
		{
			UpdateFavorites?.Invoke();
		}

		/// <summary>
		/// Обновляет список избранных.
		/// </summary>
		protected virtual void OnUpdateFavorites()
		{
			if (!IsRefreshing)
			{
				RefreshCommand.Execute(null);
			}
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
		/// Возвращает или устанавливает команду при выборе опроса.
		/// </summary>
		public Command<IGopher> EventSelected =>
			new Command<IGopher>(obj =>
			{
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});

		/// <summary>
		/// Возвращает команду для обновления списка избранных. 
		/// </summary>
		public ICommand RefreshCommand =>
			new FreshAwaitCommand((obj, tcs) =>
			{
				IsRefreshing = true;
				LoadGophers();
				IsRefreshing = false;
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает или устанавливает выбранный опрос.
		/// </summary>
		public IGopher SelectedGopher
		{
			get => _selectedGopher;
			set
			{
				if (value == null)
				{
					return;
				}
				
				_selectedGopher = value;
				RaisePropertyChanged(nameof(SelectedGopher));
				
				EventSelected.Execute(value);
				
				_selectedGopher = null;
				RaisePropertyChanged(nameof(SelectedGopher));
			}
		}
		#endregion

		#region Overrided
		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			UpdateFavorites += OnUpdateFavorites;
			_repository = new UserRepository(_app.RealmConfiguration);
			
			var user = _repository.All()
								 .SingleOrDefault();
			if (user != null)
			{
				Gophers = new ObservableCollection<IGopher>(user.FavoriteGophers);
				_service = new GopherService(user.Token);
			}
		}
		#endregion

		#region Private
		/// <summary>
		/// Загружает опросы.
		/// </summary>
		private async void LoadGophers()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var user = _repository.All()
								 .Single();
			try
			{
				Gophers = new ObservableCollection<IGopher>(await _service.GetFavorites(user));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Gophers = new ObservableCollection<IGopher>();
			}
			user.FavoriteGophers.Clear();
			foreach (var gopher in Gophers)
			{
				user.FavoriteGophers.Add((Gopher) gopher);
			}

			_repository.Update(user);
		}
		#endregion

		public bool IsEmptyList => Gophers.Count == 0;

		public override void LoadData()
		{
			RefreshCommand.Execute(null);
			IsLoaded = true;
		}
	}
}
