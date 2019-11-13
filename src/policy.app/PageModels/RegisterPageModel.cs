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
	[AddINotifyPropertyChangedInterface]
	public class RegisterPageModel : FreshBasePageModel
	{
		private int _dateOfBirthday = 2000;

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает значение поля подтверждения пароля.
		/// </summary>
		public string ConfirmPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату рождения.
		/// </summary>
		public int DateOfBirthday
		{
			get => _dateOfBirthday;
			set
			{
				if (value <= 1900 || value >= DateTime.Now.Year)
				{
					_dateOfBirthday = 0;
					MessageLabel = "Год рождения должен быть задан в промежутке между 1900 и текущим годом.";
					return;
				}

				MessageLabel = "";
				_dateOfBirthday = value;
			}
		}

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
		/// Возвращает или устанавливает email вводимый пользователем.
		/// </summary>
		public string PhoneNumber
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает команду для кнопки регистрации.
		/// </summary>
		public ICommand OnRegisterButtonClicked =>
			new FreshAwaitCommand((param, tcs) =>
			{
				if (_dateOfBirthday == 0)
				{
					return;
				}
				RegisterAsync();
				tcs.SetResult(true);
			});
		#endregion

		#region Private
		/// <summary>
		/// Вызывает регистрацию пользователя, через <see cref="IAuthService"/>, а также сохраняет нового пользователя.
		/// </summary>
		private async void RegisterAsync()
		{
			var service = new AuthService();
			var user = new User
			{
				Email = Email,
				PhoneNumber = PhoneNumber,
				Birthday = new DateTime(DateOfBirthday, 1, 1)
			};
			UserToken token = null;
			
			try
			{
				token = await service.RegisterAsync(user, Password, ConfirmPassword);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				MessageLabel = "Ошибка сервера.";
				return;
			}

			if (token == null)
			{
				MessageLabel = service.LastError;
				return;
			}

			user.Token = token;

			var app = Application.Current as App;
			if (app == null)
			{
				return;
			}

			var repository = new UserRepository(app.RealmConfiguration);

			repository.Add(user);
			app.MainPage = app.InitMainTabbedPage();
		}
		#endregion
	}
}
