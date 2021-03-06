﻿using System;
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
		#region Overridable
		/// <summary>
		/// Добавить суслика в избранное.
		/// </summary>
		/// <param name="addedGopher">Добавляемый пользователь.</param>
		/// <param name="addingGopher">Добавляющий пользователь.</param>
		/// <returns>Был ли добавлен пользователь в избранное.</returns>
		Task<bool> AddToFavorites(IGopher addedGopher, IGopher addingGopher);

		/// <summary>
		/// Возвращает все категории сусликов.
		/// </summary>
		/// <returns>Список категорий.</returns>
		Task<IEnumerable<Category>> GetCategories(Guid? uuid = null);

		/// <summary>
		/// Возвращает сусликов добавленных в избранное.
		/// </summary>
		/// <param name="gopher">Пользователь добавивший сусликов в избранное.</param>
		/// <returns>Список сусликов.</returns>
		Task<IEnumerable<IGopher>> GetFavorites(IGopher gopher);

		/// <summary>
		/// Возвращает найденных сусликов.
		/// </summary>
		/// <param name="ratingSortDirect">Направления сортировки по рейтингу.</param>
		/// <param name="query">Строка запроса, для поиска.</param>
		/// <param name="category">Искать в категории.</param>
		/// <returns>Список сусликов.</returns>
		Task<IEnumerable<IGopher>> Search(string ratingSortDirect, string query = null, Category category = null);

		/// <summary>
		/// Возвращает суслики по ид.
		/// </summary>
		/// <param name="guid">ид суслика.</param>
		/// <returns>Суслик.</returns>
		Task<Gopher> GetGopher(Guid guid);

		/// <summary>
		/// Возвращает всех сусликов категории.
		/// </summary>
		/// <param name="category">Категория, отбираемых сусликов.</param>
		/// <returns>Список сусликов по категории.</returns>
		Task<IEnumerable<IGopher>> GetGophers(Category category);

		/// <summary>
		/// Оценивает суслика.
		/// </summary>
		/// <param name="gopher">Суслик.</param>
		/// <param name="rateType">Тип оценки.</param>
		/// <returns>Был ли оценен суслик.</returns>
		Task<bool> Rate(IGopher gopher, RateType rateType);

		/// <summary>
		/// Удалить суслика из избранное.
		/// </summary>
		/// <param name="addedGopher">Удаляемый пользователь.</param>
		/// <param name="addingGopher">Удаляющий пользователь.</param>
		/// <returns>Был ли удален пользователь в избранное.</returns>
		Task<bool> RemoveFromFavorites(IGopher addedGopher, IGopher addingGopher);
		#endregion
	}
}
