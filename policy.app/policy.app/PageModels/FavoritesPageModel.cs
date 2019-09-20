using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using policy.app.Models;
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
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = Application.Current as App;
		private IGopher _selectedGopher;
		private IGopherService _service;
		private User _user;
		private string _userGuid;

		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (_app == null || !_app.IsUserLoggedIn)
			{
				return;
			}

			using (var realm = _app.Realm)
			{
				_user = realm.All<User>()?.SingleOrDefault();
				if (_user != null)
				{
					_userGuid = (string)_user.Guid.Clone();
					_service = new GopherService(new UserToken
					{
						Token = (string)_user.Token.Token.Clone(),
						TokenType = (string)_user.Token.TokenType.Clone()
					});

					Task.Run(async () =>
					{
						await Task.Delay(1000);
						LoadGophers();
					});

				}
			}
		}

		public ObservableCollection<IGopher> Gophers
		{
			get;
			set;
		}
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
		/// Возвращает или устанавливает команду при выборе опроса.
		/// </summary>
		public Command<IGopher> EventSelected =>
			new Command<IGopher>(obj => {
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});

		/// <summary>
		/// Загружает опросы.
		/// </summary>
		private async void LoadGophers()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			Gophers = new ObservableCollection<IGopher>(await _service.GetFavorites(new User
			{
				Guid = _userGuid
			}));
		}

	}
}
