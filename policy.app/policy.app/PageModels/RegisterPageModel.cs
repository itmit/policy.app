﻿using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Services;
using PropertyChanged;
using Realms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class RegisterPageModel : FreshBasePageModel
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
		/// Возвращает или устанавливает email вводимый пользователем.
		/// </summary>
		public string PhoneNumber
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
		/// Возвращает или устанавливает сообщение выводимое пользователю.
		/// </summary>
		public string MessageLabel
		{
			get;
			set;
		} = string.Empty;

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
		public DateTime DateOfBirthday
		{
			get;
			set;
		} = new DateTime(2000, 01, 01);

		/// <summary>
		/// Возвращает команду для кнопки регистрации.
		/// </summary>
		public ICommand OnRegisterButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					RegisterAsync();
				});
			}
		}

		private Realm Realm => Realm.GetInstance();
		#endregion

		private async void RegisterAsync()
		{
			var service = new AuthService();
			var user = new User()
			{
				Email = Email,
				PhoneNumber = PhoneNumber,
				Birthday = DateOfBirthday
			};

			try
			{
				user.Token = await service.RegisterAsync(user,
														 Password,
														 ConfirmPassword);
			}
			catch (AuthenticationException e)
			{
				Debug.WriteLine(e);
				MessageLabel = "Ошибка регистрации";
				return;
			}

			Realm.Write(() =>
			{
				Realm.Add(user, true);
			});

			CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
		}
	}
}
