using FreshMvvm;

namespace policy.app.PageModels
{
	public abstract class BaseMainPageModel : FreshBasePageModel
	{
		public abstract void LoadData();

		public bool IsLoaded
		{
			get;
			protected set;
		}
	}
}
