using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Services;
using PropertyChanged;
using Realms;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы изменения данных.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class EditPageModel : FreshBasePageModel
	{

		/// <summary>
		/// Возвращает или устанавливает ФИО вводимый пользователем.
		/// </summary>
		public string Name
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает город выводимое пользователю.
		/// </summary>
		public string City
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает сфера деятельности вводимое пользователем.
		/// </summary>
		public string FieldOfActivity
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает организацию.
		/// </summary>
		public string Organization
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает должность.
		/// </summary>
		public string Position
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает команду для кнопки регистрации.
		/// </summary>
		public ICommand OnSaveButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					SaveChangesAsync();
				});
			}
		}

		/// <summary>
		/// Сохраняет введенные пользователем данные.
		/// </summary>
		private void SaveChangesAsync()
		{
			var app = Application.Current as App;

			if (app == null)
			{
				return;
			}

			if (app.IsUserLoggedIn)
			{
				var realm = app.Realm;

				User user = realm.All<User>()?.SingleOrDefault();
				if (user != null)
				{
					realm.Write(() =>
					{
						user.Name = Name;
						user.City = City;
						user.FieldOfActivity = FieldOfActivity;
						user.Organization = Organization;
						user.Position = Position;
					});

					IUserService service = new UserService();

					try
					{
						service.Edit(user);
					}
					catch (AuthenticationException e)
					{
						Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка авторизации, вышел срок действия токена.", "ОК");
						Debug.WriteLine(e);
					}
					catch (NoNullAllowedException e)
					{
						Application.Current.MainPage.DisplayAlert("Ошибка", "Нет ответа от сервера.", "ОК");
						Debug.WriteLine(e);
					}
				}
			}
		}
	}
}
