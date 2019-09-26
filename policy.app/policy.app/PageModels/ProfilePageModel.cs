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
using Realms;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class ProfilePageModel : FreshBasePageModel
	{
		private App _app;

		#region .ctor
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		/// <param name="initData"></param>
		public override void Init(object initData)
		{
			base.Init(initData);

			CheckPermissionStorage();

			_app = Application.Current as App;
			if (_app == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var users = repository
				.All();
			User = users.SingleOrDefault();
			if (User != null && string.IsNullOrEmpty(User.PhotoSource))
			{
				User.PhotoSource = "def_profile";
			}
		}

		private async void CheckPermissionStorage()
		{
			await CheckPermission(Permission.Storage, "Для загрузки аватара необходимо разрешение на использование хранилища.");
		}

		public User User
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает должность пользователя.
		/// </summary>
		public string Position
		{
			get;
			set;
		}
		#endregion

		#region Properties

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

		/// <summary>
		/// Возвращает команду для установки аватара.
		/// </summary>
		public FreshAwaitCommand SetAvatarCommand => new FreshAwaitCommand(async (obj, tcs) =>
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
			tcs.SetResult(true);
		});

		/// <summary>
		/// Возвращает команду для открытия страницы редактирования данных пользователя.
		/// </summary>
		public ICommand OpenEditPage
		{
			get => new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<EditPageModel>();
				tcs.SetResult(true);
			});
		}

		#endregion
	}
}
