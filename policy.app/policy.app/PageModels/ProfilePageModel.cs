using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class ProfilePageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		private readonly App _app = App.Current;
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
			if (!IsRefreshing)
			{
				RefreshCommand.Execute(null);
			}
		}

		/// <summary>
		/// Возвращает или устанавливает должность пользователя.
		/// </summary>
		public string Position
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает пользователя.
		/// </summary>
		public User User
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает команду для открытия страницы редактирования данных пользователя.
		/// </summary>
		public ICommand OpenEditPage =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<EditPageModel>();
				tcs.SetResult(true);
			});


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
			new FreshAwaitCommand((obj, tcs) =>
			{
				IsRefreshing = true;

				var repository = new UserRepository(_app.RealmConfiguration);
				var users = repository.All();
				User = users.SingleOrDefault();
				if (User != null && string.IsNullOrEmpty(User.PhotoSource))
				{
					User.PhotoSource = "def_profile";
				}
				IsRefreshing = false;
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для установки аватара.
		/// </summary>
		public FreshAwaitCommand SetAvatarCommand =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				if (await CheckPermission(Permission.Storage, "Для загрузки аватара необходимо разрешение на использование хранилища."))
				{
					if (!CrossMedia.Current.IsPickPhotoSupported)
					{
						return;
					}

					var image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
					{
						PhotoSize = PhotoSize.Medium
					});

					if (image == null)
					{
						return;
					}

					User.PhotoSource = image.Path;

					var repo = new UserRepository(_app.RealmConfiguration);
					repo.Update(User);

					using (var memoryStream = new MemoryStream())
					{
						image.GetStream()
							 .CopyTo(memoryStream);
						image.Dispose();
						IUserService service = new UserService();
						service.ChangeUserAvatarPhoto(User, memoryStream.ToArray());
					}
				}
				RefreshCommand.Execute(null);
				MenuPageModel.InvokeUpdateUser();
				tcs.SetResult(true);
			});
		#endregion

		#region Overrided
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		/// <param name="initData">Параметр модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			CheckPermissionStorage();

			UpdateUser += OnUpdateUser;

			RefreshCommand.Execute(null);
		}
		#endregion

		#region Private
		/// <summary>
		/// Проверяет разрешения.
		/// </summary>
		/// <param name="permission">Разрешение.</param>
		/// <param name="message">Сообщение показываемое пользователю при отклонении.</param>
		/// <returns>Было ли получено разрешение.</returns>
		private async Task<bool> CheckPermission(Permission permission, string message)
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
			if (status != PermissionStatus.Granted)
			{
				if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
				{
					await Application.Current.MainPage.DisplayAlert("Внимание", message, "OK");
				}

				await CrossPermissions.Current.RequestPermissionsAsync(permission);

				status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
			}

			return await Task.FromResult(status == PermissionStatus.Granted);
		}

		private async void CheckPermissionStorage()
		{
			await CheckPermission(Permission.Storage, "Для загрузки аватара необходимо разрешение на использование хранилища.");
		}
		#endregion
	}
}
