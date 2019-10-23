using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы изменения данных.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class EditPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Возвращает текущий <see cref="Application" />.
		/// </summary>
		private App _app;
		#endregion
		#endregion

		#region Properties
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
		/// Возвращает или устанавливает ФИО вводимый пользователем.
		/// </summary>
		public string Name
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
		 =>  new FreshAwaitCommand((param, tcs) =>
				{
					SaveChangesAsync();
					ProfilePageModel.InvokeUpdateUser();
					MenuPageModel.InvokeUpdateUser();
				});
		#endregion

		#region Overrided
		/// <summary>
		/// Вызывается при загрузке модели представления.
		/// </summary>
		/// <param name="initData">Данные, которые были отправлены из модели представления ранее.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			_app = Application.Current as App;

			if (_app != null)
			{
				var repository = new UserRepository(_app.RealmConfiguration);

				var user = repository.All()
									 .SingleOrDefault();
				if (user == null)
				{
					return;
				}

				Name = user.Name;
				City = user.City;
				FieldOfActivity = user.FieldOfActivity;
				Organization = user.Organization;
				Position = user.Position;
			}
		}
		#endregion

		#region Private
		/// <summary>
		/// Сохраняет введенные пользователем данные.
		/// </summary>
		private void SaveChangesAsync()
		{
			if (_app == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);

				var user = repository.All()
									 .SingleOrDefault();
				if (user != null)
				{
					user.Name = Name;
					user.City = City;
					user.FieldOfActivity = FieldOfActivity;
					user.Organization = Organization;
					user.Position = Position;
					repository.Update(user);

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
		#endregion
	}
}
