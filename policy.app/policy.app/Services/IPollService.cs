using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для работы с опросами по api.
	/// </summary>
	public interface IPollService
	{
		/// <summary>
		/// Возвращает список опросов.
		/// </summary>
		/// <returns>Список опросов.</returns>
		Task<IEnumerable<Poll>> GetPolls();

		/// <summary>
		/// Возвращает список вопросов опроса.
		/// </summary>
		/// <param name="guid">Ид опроса.</param>
		/// <returns>Список вопросов.</returns>
		Task<IEnumerable<Question>> GetQuestions(Guid guid);
	}
}
