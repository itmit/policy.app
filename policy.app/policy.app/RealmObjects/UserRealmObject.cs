using System;
using System.Collections.Generic;
using policy.app.Models;
using Realms;

namespace policy.app.RealmObjects
{
	public class UserRealmObject : RealmObject
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает дату рождения пользователя.
		/// </summary>
		public DateTimeOffset Birthday
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает категорию.
		/// </summary>
		public CategoryRealmObject Category
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
		/// Возвращает или устанавливает количество отрицательных оценок.
		/// </summary>
		public int Dislikes
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает почтовый адрес пользователя.
		/// </summary>
		public string Email
		{
			get;
			set;
		}

		public IList<GopherRealmObject> FavoriteGophers
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает область деятельности пользователя.
		/// </summary>
		public string FieldOfActivity
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид пользователя.
		/// </summary>
		[PrimaryKey]
		public string Guid
		{
			get;
			set;
		} = System.Guid.NewGuid()
				  .ToString();

		/// <summary>
		/// Возвращает или устанавливает количество положительных оценок.
		/// </summary>
		public int Likes
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
		/// Возвращает или устанавливает количество нейтральных оценок.
		/// </summary>
		public int Neutrals
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
		/// Возвращает или задает номер телефона пользователя.
		/// </summary>
		public string PhoneNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает источник фотографии.
		/// </summary>
		public string PhotoSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		public string PlaceOfWork
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
		/// Возвращает или устанавливает токен пользователя.
		/// </summary>
		public TokenRealmObject Token
		{
			get;
			set;
		}
		#endregion
	}
}
