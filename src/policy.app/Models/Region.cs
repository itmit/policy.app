using System;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель региона.
	/// </summary>
	public class Region
	{
		/// <summary>
		/// Возвращает или устанавливает ид региона.
		/// </summary>
		[JsonProperty("uuid")]
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает номер региона.
		/// </summary>
		[JsonProperty("id")]
		public int Number
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает название региона.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		public string ListName => $"{Number} {Name}";

		/// <summary>
		/// Возвращает или устанавливает дату создания.
		/// </summary>
		public DateTime? CreatedAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату обновления.
		/// </summary>
		public DateTime? UpdateAt
		{
			get;
			set;
		}
	}
}
