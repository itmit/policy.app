using System;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель категории пользователей.
	/// </summary>
	public class Category
	{
		#region Properties
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
		/// Возвращает или устанавливает ид категории.
		/// </summary>
		public Guid Uuid
		{
			get;
			set;
		} = Guid.NewGuid();
		#endregion
	}
}
