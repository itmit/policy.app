using System;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для отзывов пользователя
	/// </summary>
	public class Feedback
	{
		/// <summary>
		/// Возвращает или устанавливает ид отзыва.
		/// </summary>
		public Guid Guid
		{
			get;
			set;
		} = Guid.NewGuid();

		/// <summary>
		/// Возвращает или устанавливает заголовок/тему отзыва.
		/// </summary>
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает сообщение отзыва.
		/// </summary>
		public string Message
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает категорию отзыва.
		/// </summary>
		public Category Category
		{
			get;
			set;
		}
	}
}
