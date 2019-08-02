using System;
using System.Collections.Generic;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для api пользователя.
	/// </summary>
	public class UserService : IUserService
	{
		private const string EditUri = "http://policy.itmit-studio.ru/api/user/edit";

		/// <summary>
		/// Возвращает всех пользователей.
		/// </summary>
		/// <returns>Список пользователей.</returns>
		public IEnumerable<User> GetAllAsync() => throw new NotImplementedException();

		/// <summary>
		/// Возвращает пользователя по <see cref="Guid"/>.
		/// </summary>
		/// <param name="guid">Ид по которому производится поиск.</param>
		/// <returns>Пользователь, с ид указанным в параметре.</returns>
		public User GetUserByGuid(Guid guid) => throw new NotImplementedException();

		/// <summary>
		/// Сохраняет измененные данные пользователя.
		/// </summary>
		/// <param name="user">Пользователь, чьи измененные данные необходимо сохранить.</param>
		public void Edit(User user)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Устанавливает фотографию для аватара, пользователя.
		/// </summary>
		/// <param name="user">Пользователь, которому необходимо установить аватар.</param>
		/// <param name="file"><see cref="Uri"/> файла, которое нужно установить в качестве аватара пользователя.</param>
		public void ChangeUserAvatarPhoto(User user, Uri file)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Отправляет форму обратной связи.
		/// </summary>
		/// <param name="user">Пользователь, от имени которого отправляется форма.</param>
		/// <param name="feedback">Данные из формы обратной связи.</param>
		public void SendFeedBack(User user, Feedback feedback)
		{
			throw new NotImplementedException();
		}
	}
}
