using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.PageModels;
using PropertyChanged;

namespace policy.app.ViewModel
{
	[AddINotifyPropertyChangedInterface]
	public class SearchGopherViewModel
	{
		private FreshBasePageModel _parent;

		public SearchGopherViewModel(IGopher gopher, FreshBasePageModel parent)
		{
			_parent = parent;
			Gopher = gopher;
		}

		public ICommand SelectCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				_parent.CoreMethods.PushPageModel<UserPageModel>(Gopher);
				tcs.SetResult(true);
			});

		public IGopher Gopher
		{
			get;
		}
	}
}
