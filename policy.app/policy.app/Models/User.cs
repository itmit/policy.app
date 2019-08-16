using System;
using Newtonsoft.Json;
using PropertyChanged;
using Realms;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для сущности пользователя.
	/// </summary>
	public class User : RealmObject
	{
		/// <summary>
		/// Возвращает или устанавливает ид пользователя.
		/// </summary>
		[JsonProperty("uid")]
		public string Guid
		{
			get;
			set;
		} = System.Guid.NewGuid()
				.ToString();

		/// <summary>
		/// Возвращает или устанавливает почтовый адрес пользователя.
		/// </summary>
		public string Email
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает токен пользователя.
		/// </summary>
		public UserToken Token
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает город пользователя.
		/// </summary>
		public string City
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает область деятельности пользователя.
		/// </summary>
		[JsonProperty("field_of_activity")]
		public string FieldOfActivity
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает организацию пользователя.
		/// </summary>
		public string Organization
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает должность пользователя.
		/// </summary>
		public string Position
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату рождения пользователя.
		/// </summary>
		public DateTimeOffset Birthday
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или задает номер телефона пользователя.
		/// </summary>
		[JsonProperty("phone_number")]
		public string PhoneNumber
		{
			get;
			set;
		}
	}
}
