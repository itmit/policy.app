using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для вопросов.
	/// </summary>
	public class Question
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает список ответов.
		/// </summary>
		public List<Answer> Answers
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает время создания вопроса.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime CreatedAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид вопроса.
		/// </summary>
		[JsonProperty("question_uuid")]
		public Guid Guid
		{
			get;
			set;
		} = Guid.NewGuid();

		/// <summary>
		/// Возвращает или устанавливает является ли вопрос с множественным вариантом ответа.
		/// </summary>
		public bool Multiple
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает текст вопроса.
		/// </summary>
		[JsonProperty("question")]
		public string QuestionText
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает время обновления вопроса.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTime UpdatedAt
		{
			get;
			set;
		}
		#endregion
	}
}
