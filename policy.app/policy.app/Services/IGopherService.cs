using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для работы с сусликами и их категориями.
	/// </summary>
	public interface IGopherService
	{
		/// <summary>
		/// Возвращает все категории сусликов.
		/// </summary>
		/// <returns>Список категорий.</returns>
		Task<IEnumerable<Category>> GetCategories();

		/// <summary>
		/// Возвращает всех сусликов категории.
		/// </summary>
		/// <param name="category">Категория, отбираемых сусликов.</param>
		/// <returns>Список сусликов по категории.</returns>
		Task<IEnumerable<Gopher>> GetGophers(Category category);

		/// <summary>
		/// Возвращает суслики по ид.
		/// </summary>
		/// <param name="guid">ид суслика.</param>
		/// <returns>Суслик.</returns>
		Task<Gopher> GetGopher(Guid guid);
	}
}
