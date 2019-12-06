using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы авторизации.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class LoginPageModel : FreshBasePageModel
	{
		private bool _isBusy;

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
		public ICommand OnRegisterButtonClicked =>
			new FreshAwaitCommand((param, tcs) =>
			{
				if (_isBusy)
				{
					return;
				}
				_isBusy = true;
				CoreMethods.PushPageModel<RegisterPageModel>();
				_isBusy = false;
				tcs.SetResult(true);
			});
		#endregion

		#region Private
		/// <summary>
		/// Авторизует пользователя при вызове соответствующей команды.
		/// </summary>
		private async void OnLoginClicked()
		{
			if (_isBusy)
			{
				return;
			}
			
			_isBusy = true;
			User user;
			var service = new AuthService();
			
			try
			{
				user = await service.GetUserByTokenAsync(await service.LoginAsync(Email, Password));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				_isBusy = false;
				MessageLabel = "Неверно указаны email или пароль.";
				return;
			}

			if (user == null)
			{
				MessageLabel = service.LastError;
				return;
			}

			var app = Application.Current as App;

			if (app == null)
			{
				_isBusy = false;
				return;
			}

			_isBusy = false;
			var repository = new UserRepository(app.RealmConfiguration);
			repository.Add(user);
			app.MainPage = app.InitMainTabbedPage();
		}
		#endregion
	}
}
