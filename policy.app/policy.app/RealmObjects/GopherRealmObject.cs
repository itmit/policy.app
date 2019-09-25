using policy.app.Models;
using Realms;

namespace policy.app.RealmObjects
{
	public class GopherRealmObject : RealmObject
	{
		/// <summary>
		/// Возвращает или устанавливает категорию.
		/// </summary>
		public CategoryRealmObject Category
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает количество отрицательных оценок.
		/// </summary>
		public int Dislikes
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид пользователя.
		/// </summary>
		[PrimaryKey]
		public string Guid
		{
			get;
			set;
		} = System.Guid.NewGuid()
				  .ToString();

		/// <summary>
		/// Возвращает или устанавливает количество положительных оценок.
		/// </summary>
		public int Likes
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает количество нейтральных оценок.
		/// </summary>
		public int Neutrals
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает источник фотографии.
		/// </summary>
		public string PhotoSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
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
	}
}
