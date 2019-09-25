using Realms;

namespace policy.app.RealmObjects
{
	public class TokenRealmObject : RealmObject
	{

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает токен для авторизации.
		/// </summary>
		public string Token
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает строковое представление даты, до которой действует токен.
		/// </summary>
		public string TokenExpiresAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает тип токена.
		/// </summary>
		public string TokenType
		{
			get;
			set;
		}
		#endregion
	}
}
