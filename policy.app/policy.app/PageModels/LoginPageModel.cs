using System.Diagnostics;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Realms;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы авторизации.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class LoginPageModel : FreshBasePageModel
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает email вводимый пользователем.
		/// </summary>
		public string Email
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает сообщение выводимое пользователю.
		/// </summary>
		public string MessageLabel
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает пароль вводимый пользователем.
		/// </summary>
		public string Password
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает команду для авторизации.
		/// </summary>
		public ICommand OnLoginButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					OnLoginClicked();
					tcs.SetResult(true);
				});
			}
		}

		/// <summary>
		/// Возвращает команду для кнопки регистрации.
		/// </summary>
		public ICommand OnRegisterButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					CoreMethods.PushPageModel<RegisterPageModel>();
				});
			}
		}

		#endregion

		#region Private
		/// <summary>
		/// Авторизует пользователя при вызове соответствующей команды.
		/// </summary>
		private async void OnLoginClicked()
		{
			User user;
			try
			{
				var service = new AuthService();

				user = await service.GetUserByTokenAsync(await service.LoginAsync(Email, Password));
			}
			catch (AuthenticationException e)
			{
				Debug.WriteLine(e);
				MessageLabel = "Неверно указаны email или пароль";
				return;
			}

			var app = Application.Current as App;

			if (app == null)
			{
				return;
			}

			app.IsUserLoggedIn = true;
			var repository = new UserRepository(app.RealmConfiguration);

			repository.Add(user);

			app.MainPage = app.InitMainTabbedPage();
			
		}
		#endregion
	}
}
