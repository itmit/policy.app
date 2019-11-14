using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using PropertyChanged;

namespace policy.app.ViewModel
{
	/// <summary>
	/// Представляет модель представления для суслика на странице поиска. 
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class SearchGopherViewModel
	{
		/// <summary>
		/// Базовая модель представления.
		/// </summary>
		private FreshBasePageModel _parent;

		public SearchGopherViewModel(IGopher gopher, FreshBasePageModel parent)
		{
			_parent = parent;
			Gopher = gopher;
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
		/// Возвращает команду для выбора суслика.
		/// </summary>
		public ICommand SelectCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				_parent.CoreMethods.PushPageModel<UserPageModel>(Gopher);
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает суслика.
		/// </summary>
		public IGopher Gopher
		{
			get;
		}
	}
}
