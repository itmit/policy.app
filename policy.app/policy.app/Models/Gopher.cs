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
		[JsonProperty("uuid")]
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

		public Category Category
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

		public int Likes
		{
			get;
			set;
		}

		public int Neutrals
		{
			get;
			set;
		}

		public int Dislikes
		{
			get;
			set;
		}
	}
}
