﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для опросов.
	/// </summary>
	public class Poll
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает дату создания опроса.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime? CreatedAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату конца опроса.
		/// </summary>
		[JsonProperty("end_at")]
		public DateTime? EndAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид опроса.
		/// </summary>
		[JsonProperty("uuid")]
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает название опроса.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает номер опроса в списке опросов.
		/// </summary>
		public int PollListNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает список вопросов.
		/// </summary>
		public List<Question> Questions
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату начала опроса.
		/// </summary>
		[JsonProperty("start_at")]
		public DateTime? StartAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату обновления опроса.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTime? UpdatedAt
		{
			get;
			set;
		}
		#endregion
	}
}
