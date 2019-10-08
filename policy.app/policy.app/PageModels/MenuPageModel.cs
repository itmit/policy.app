using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using policy.app.Repositories;
using PropertyChanged;
using Xamarin.Forms;
using MenuItem = policy.app.Models.MenuItem;

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
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = Application.Current as App;
		/// <summary>
		/// Определяет выбранный пункт в <see cref="Xamarin.Forms.ListView" />.
		/// </summary>
		private MenuItem _selectedItem;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Представляет метод обновления профиля.
		/// </summary>
		public delegate void UpdateUserEventHandler();

		/// <summary>
		/// Происходит после обновлений данных у пользователя.
		/// </summary>
		public static event UpdateUserEventHandler UpdateUser;

		/// <summary>
		/// Провоцирует событие <see cref="UpdateUser"/>.
		/// </summary>
		public static void InvokeUpdateUser()
		{
			UpdateUser?.Invoke();
		}

		/// <summary>
		/// Обновляет данные пользователя в профиле.
		/// </summary>
		protected virtual void OnUpdateUser()
		{
			UpdateUserData();
		}

		/// <summary>
		/// Возвращает список пунктов меню для домашней страницы.
		/// </summary>
		public ObservableCollection<MenuItem> MenuCollection
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string UserPicture
		{
			get;
			set;
		}

		public Command<MenuItem> EventSelected =>
			new Command<MenuItem>(obj =>
			{
				if (obj is MenuItem menuItem)
				{
					if (menuItem.PageModelType == null)
					{
						menuItem.Execute();
						return;
					}

					CoreMethods.PushPageModel(menuItem.PageModelType, false);
				}
			});

		/// <summary>
		/// Возвращает или устанавливает выбранный пункт меню.
		/// </summary>
		public MenuItem SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;

				if (value != null)
				{
					EventSelected.Execute(value);
				}
			}
		}
		#endregion

		#region Public
		public void UpdateUserData()
		{
			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .Single();

			UserName = string.IsNullOrEmpty(user.Name) ? "Гость" : user.Name;
			UserPicture = string.IsNullOrEmpty(user.PhotoSource) ? "User_def" : user.PhotoSource;
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

			UpdateUser += OnUpdateUser;

			if (_app == null || !_app.IsUserLoggedIn)
			{
				return;
			}

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
				new MenuItem("О приложении", typeof(AboutPageModel))
				{
					ImageSource = "menu_4_def.png"
				},
				new MenuItem("Выход",
							 () =>
							 {
								 var rep = new UserRepository(_app.RealmConfiguration);
								 rep.Remove(rep.All()
											   .Single());
								 CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.AuthenticationContainer);
							 })
				{
					ImageSource = "menu_5_def.png"
				}
			};
			UpdateUserData();
		}
		#endregion
	}
}
