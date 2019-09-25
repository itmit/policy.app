using System.Linq;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using PropertyChanged;
using Realms;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class ProfilePageModel : FreshBasePageModel
	{
		#region .ctor
		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		public ProfilePageModel()
		{
			var app = Application.Current as App;
			if (app == null)
			{
				return;
			}

			var repository = new UserRepository(app.RealmConfiguration);
			var users = repository
							 .All();
			var user = users.SingleOrDefault();

			if (user != null)
			{
				Name = user.Name;
				City = user.City;
				Activity = user.FieldOfActivity;
				Organization = user.Organization;
				Position = user.Position;
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
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает сферу деятельности текущего пользователя.
		/// </summary>
		public string Activity
		{
			get;
			set;
		}

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

		/// <summary>
		/// Возвращает или устанавливает город текущего пользователя.
		/// </summary>
		public string City
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя текущего пользователя.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает организацию текущего пользователя.
		/// </summary>
		public string Organization
		{
			get;
			set;
		}
		#endregion
	}
}
