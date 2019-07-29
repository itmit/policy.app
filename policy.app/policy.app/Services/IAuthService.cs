using System.Security.Authentication;
using System.Threading.Tasks;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для авторизации.
	/// </summary>
	public interface IAuthService
	{
		#region Overridable
		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token">Токен для получения пользователя</param>
		/// <returns>Авторизованный пользователь.</returns>
		Task<User> GetUserByTokenAsync(UserToken token);

		/// <summary>
		/// Отправляет запрос на авторизацию, по api.
		/// </summary>
		/// <param name="login">Логин для авторизации.</param>
		/// <param name="pass">Пароль для авторизации.</param>
		/// <returns>Токен авторизованного пользователя.</returns>
		/// <exception cref="AuthenticationException">Возникает при неудачной авторизации.</exception>
		Task<UserToken> LoginAsync(string login, string pass);

		/// <summary>
		/// Отправляет запрос для регистрации, при помощи rest api.
		/// </summary>
		/// <param name="user">Пользователь, которого необходимо зарегистрировать.</param>
		/// <param name="password">Пароль пользователя.</param>
		/// <param name="confirmPassword">подтверждение пароля.</param>
		/// <returns>Токен нового пользователя.</returns>
		Task<UserToken> RegisterAsync(User user, string password, string confirmPassword);
		#endregion
	}
}
