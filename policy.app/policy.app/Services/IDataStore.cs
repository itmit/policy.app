using System.Collections.Generic;
using System.Threading.Tasks;

namespace policy.app.Services
{
	public interface IDataStore<T>
	{
		#region Overridable
		Task<bool> AddItemAsync(T item);

		Task<bool> DeleteItemAsync(string id);

		Task<T> GetItemAsync(string id);

		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task<bool> UpdateItemAsync(T item);
		#endregion
	}
}
