using Newtonsoft.Json;

namespace policy.app.Models
{
	/// <summary>
	/// Представляет тип для хранения данных токене авторизации пользователя.
	/// </summary>
	public class UserToken
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает токен для авторизации.
		/// </summary>
		[JsonProperty("access_token")]
		public string Token
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает строковое представление даты, до которой действует токен.
		/// </summary>
		[JsonProperty("expires_at")]
		public string TokenExpiresAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает тип токена.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType
		{
			get;
			set;
		}
		#endregion
	}
}
