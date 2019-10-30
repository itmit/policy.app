using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Essentials;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class WriteToUsPageModel : FreshBasePageModel
	{
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;

		/// <summary>
		/// Сервис для работы с сусликами.
		/// </summary>
		private IGopherService _service;

		/// <summary>
		/// Репозиторий для работы с данными пользователя.
		/// </summary>
		private UserRepository _repository;

		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		/// <param name="initData">Параметр модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			_repository = new UserRepository(_app.RealmConfiguration);
			var user = _repository.All()
								   .SingleOrDefault();
			if (user != null)
			{
				_service = new GopherService(user.Token);
				LoadCategories();
			}
		}

		public FreshAwaitCommand SendCommand 
			=> new FreshAwaitCommand(async (obj, tcs) =>
			{
				IUserService service = new UserService();
				var user = _repository.All()
									  .SingleOrDefault();

				if (user == null)
				{
					return;
				}

				if (SelectCategory == null || string.IsNullOrEmpty(Theme) || string.IsNullOrEmpty(Text))
				{
					await _app.MainPage.DisplayAlert("Уведомление", "Все поля должны быть заполнены.", "Ok");
					return;
				}

				var res = await service.SendFeedBack(user, new Feedback
				{
					Category = SelectCategory,
					Message = Text,
					Title = Theme
				});

				if (res)
				{
					await CoreMethods.PopPageModel();
					await _app.MainPage.DisplayAlert("Уведомление", "Форма отправлена.", "Ok");
				}
				else
				{
					await _app.MainPage.DisplayAlert("Уведомление", "Ошибка отправки.", "Ok");
				}
			});


		public Category SelectCategory
		{
			get;
			set;
		}

		private async void LoadCategories()
		{
			if (_service == null || Connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return;
			}

			Categories = new ObservableCollection<Category>(await _service.GetCategories());
		}

		public ObservableCollection<Category> Categories
		{
			get;
			set;
		}

		public string Theme
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}
	}
}
