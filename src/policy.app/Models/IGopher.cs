using System;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет интерфейс для сусликов.
	/// </summary>
	public interface IGopher
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает категорию.
		/// </summary>
		Category Category
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает ссылку с информацией о суслике. 
		/// </summary>
		string Link
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает количество отрицательных оценок.
		/// </summary>
		int Dislikes
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает ид пользователя.
		/// </summary>
		Guid Guid
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает количество положительных оценок.
		/// </summary>
		int Likes
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает количество нейтральных оценок.
		/// </summary>
		int Neutrals
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает источник фотографии.
		/// </summary>
		string PhotoSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		string PlaceOfWork
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает должность пользователя.
		/// </summary>
		string Position
		{
			get;
		}
		#endregion
	}
}
