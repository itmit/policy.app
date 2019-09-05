using FreshMvvm;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы "О нас".
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class AboutPageModel: FreshBasePageModel
	{
		public FreshAwaitCommand AskCommand 
			=> new FreshAwaitCommand((param, tcs) =>
			{
				// TODO: Написать функционал для задания вопроса.
				tcs.SetResult(true);
			});
	}
}
