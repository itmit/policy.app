using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель категории пользователей.
	/// </summary>
	public class Category
	{
		/// <summary>
		/// Возвращает или устанавливает название категории пользователей.
		/// </summary>
		[JsonProperty("name")]
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает пользователей категории.
		/// </summary>
		public List<User> Users
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид категории. 
		/// </summary>
		public string Uuid
		{
			get;
			set;
		} = Guid.NewGuid().ToString();
	}
}
