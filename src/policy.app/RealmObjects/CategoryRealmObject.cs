using System;
using Realms;

namespace policy.app.RealmObjects
{
	public class CategoryRealmObject : RealmObject
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает название категории пользователей.
		/// </summary>
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид категории.
		/// </summary>
		public string Uuid
		{
			get;
			set;
		} = Guid.NewGuid()
				.ToString();
		#endregion
	}
}
