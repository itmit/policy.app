using System;
using Xamarin.Forms;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет модель для пункта меню.
	/// </summary>
	public class MenuItem
	{
		/// <summary>
		/// Возвращает или устанавливает заголовок пункта меню.
		/// </summary>
		public string Title
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
			set;
		}
        /// <summary>
        /// Путь изображения меню
        /// </summary>
        public string ImageSource
        {
            get;
            set;
        }

    }
}
