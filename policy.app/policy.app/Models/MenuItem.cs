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
		/// Возвращает или устанавливает страницу для перехода.
		/// </summary>
		public Page Page
		{
			get;
			set;
		}

	}
}
