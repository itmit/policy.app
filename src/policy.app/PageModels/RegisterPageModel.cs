using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;
using Region = policy.app.Models.Region;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class RegisterPageModel : FreshBasePageModel
	{
		private int _dateOfBirthday = 2000;

		public override async void Init(object initData)
		{
			base.Init(initData);

			var service = new AuthService();
			Regions = new ObservableCollection<Region>(await service.GetRegions());
		}

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает регионы.
		/// </summary>
		public ObservableCollection<Region> Regions
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает регион.
		/// </summary>
		public Region Region
		{
			get;
			set;
		}

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
				Birthday = new DateTime(DateOfBirthday, 1, 1),
				Education = Education,
				SettlementType = SettlementType,
				Region = Region,
				Gender = Gender

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

		public string Gender
		{
			get;
			set;
		}

		public string SettlementType
		{
			get;
			set;
		}

		public string Education
		{
			get;
			set;
		}
		#endregion
	}
}
