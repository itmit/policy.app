using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
	/// Представляет модель представления для страницы списка избранных сусликов.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class FavoritesPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;

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

		public static FavoritesPageModel Instance
		{
			get;
			private set;
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

		public FreshAwaitCommand RefreshCommand =>
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
				_selectedGopher = value;

				if (value != null)
				{
					EventSelected.Execute(value);
				}
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
			Instance = this;
			if (_app == null || !_app.IsUserLoggedIn)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .SingleOrDefault();
			if (user != null)
			{
				_service = new GopherService(new UserToken
				{
					Token = (string) user.Token.Token.Clone(),
					TokenType = (string) user.Token.TokenType.Clone()
				});

				Task.Run(async () =>
				{
					await Task.Delay(1000);
					LoadGophers();
				});
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

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .Single();
			Gophers = new ObservableCollection<IGopher>(await _service.GetFavorites(user));
			user.FavoriteGophers.Clear();
			foreach (var gopher in Gophers)
			{
				user.FavoriteGophers.Add((Gopher) gopher);
			}

			repository.Update(user);
		}
		#endregion
	}
}
