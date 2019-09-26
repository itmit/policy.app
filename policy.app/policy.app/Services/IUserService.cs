using System;
using System.Collections.Generic;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для api пользователя.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Возвращает всех пользователей.
		/// </summary>
		/// <returns>Список пользователей.</returns>
		IEnumerable<User> GetAllAsync();

		/// <summary>
		/// Возвращает пользователя по <see cref="Guid"/>.
		/// </summary>
		/// <param name="guid">Ид по которому производится поиск.</param>
		/// <returns>Пользователь, с ид указанным в параметре.</returns>
		User GetUserByGuid(Guid guid);

		/// <summary>
		/// Сохраняет измененные данные пользователя.
		/// </summary>
		/// <param name="user">Пользователь, чьи измененные данные необходимо сохранить.</param>
		void Edit(User user);

		/// <summary>
		/// Устанавливает фотографию для аватара, пользователя.
		/// </summary>
		/// <param name="user">Пользователь, которому необходимо установить аватар.</param>
		/// <param name="image">Файл, которое нужно установить в качестве аватара пользователя.</param>
		void ChangeUserAvatarPhoto(User user, byte[] image);

		/// <summary>
		/// Отправляет форму обратной связи.
		/// </summary>
		/// <param name="user">Пользователь, от имени которого отправляется форма.</param>
		/// <param name="feedback">Данные из формы обратной связи.</param>
		void SendFeedBack(User user, Feedback feedback);
	}
}
