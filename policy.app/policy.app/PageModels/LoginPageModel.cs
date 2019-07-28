﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using FreshMvvm;
using Newtonsoft.Json;
using policy.app.Models;
using policy.app.Services;
using PropertyChanged;
using Realms;

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

		private Realm Realm => Realm.GetInstance();
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

			App.IsUserLoggedIn = true;

			Realm.Write(() =>
			{
				Realm.Add(user, true);
			});

			CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
		}
		#endregion
	}
}