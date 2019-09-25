using System;
using static System.String;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для пункта меню.
	/// </summary>
	public class MenuItem
	{
		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="MenuItem" />.
		/// </summary>
		/// <param name="title">Заголовок пункта меню.</param>
		/// <param name="pageModelType">Тип модели представления, который будет открыт при нажатии.</param>
		public MenuItem(string title, Type pageModelType)
		{
			if (IsNullOrEmpty(title))
			{
				throw new ArgumentNullException(title);
			}

			Title = title;
			PageModelType = pageModelType;
		}

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="MenuItem" />.
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="title">Заголовок пункта меню.</param>
		/// <param name="pageModelType">Тип модели представления, который будет открыт при нажатии.</param>
		public MenuItem(object parameters, string title, Type pageModelType)
		{
			if (IsNullOrEmpty(title))
			{
				throw new ArgumentNullException(title);
			}

			Parameters = parameters;

			Title = title;
			PageModelType = pageModelType;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает путь изображения меню.
		/// </summary>
		public string ImageSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает тип модели страницы для перехода.
		/// </summary>
		public Type PageModelType
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает параметры, которые необходимы для модели представления.
		/// </summary>
		public object Parameters
		{
			get;
		}

		/// <summary>
		/// Возвращает или устанавливает заголовок пункта меню.
		/// </summary>
		public string Title
		{
			get;
		}
		#endregion
	}
}
