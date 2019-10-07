using System;
using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для ответов.
	/// </summary>
	public class Answer
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает текст вопроса.
		/// </summary>
		[JsonProperty("answer")]
		public string AnswersText
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид ответа.
		/// </summary>
		[JsonProperty("answer_uuid")]
		public Guid Guid
		{
			get;
			set;
		} = Guid.NewGuid();

		/// <summary>
		/// Возвращает или устанавливает является ли ответ "другим".
		/// </summary>
		[JsonProperty("type")]
		public bool IsOther
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает выбранный ли ответ.
		/// </summary>
		public bool IsSelected
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает текст "другого" ответа, введенный пользователем.
		/// </summary>
		[JsonProperty("other")]
		public string OtherText
		{
			get;
			set;
		}
		#endregion
	}
}
