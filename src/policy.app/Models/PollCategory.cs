using System;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для категории опросов.
	/// </summary>
	public class PollCategory
	{
		/// <summary>
		/// Возвращает или устанавливает ид категории сусликов.
		/// </summary>
		[JsonProperty("uuid")]
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает источник картинки.
		/// </summary>
		public string ImageSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает название категории.
		/// </summary>
		public string Name
		{
			get;
			set;
		}
	}
}
