using Newtonsoft.Json;
using Realms;

namespace policy.app.Models
{
	public class Gopher : IGopher
	{
		/// <summary>
		/// Возвращает или устанавливает ид пользователя.
		/// </summary>
		[PrimaryKey]
		[JsonProperty("uid")]
		public string Guid
		{
			get;
			set;
		} = System.Guid.NewGuid()
				  .ToString();

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		[JsonProperty("place_of_work")]
		public string PlaceOfWork
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает должность пользователя.
		/// </summary>
		public string Position
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает источник фотографии.
		/// </summary>
		[JsonProperty("photo")]
		public string PhotoSource
		{
			get;
			set;
		}
	}
}
