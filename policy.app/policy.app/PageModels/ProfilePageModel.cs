using System.Linq;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;
using Realms;

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
			var users = Realm.GetInstance()
							 .All<User>();
			var user = users?.SingleOrDefault();

			if (user != null)
			{
				Name = user.Name;
				City = user.City;
				Activity = user.FieldOfActivity;
				Organization = user.Organization;
			}
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
