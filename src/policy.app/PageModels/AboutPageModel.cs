using FreshMvvm;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы "О нас".
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class AboutPageModel : FreshBasePageModel
	{
		#region Properties
		/// <summary>
		/// Возвращает команду для перехода на страницу "Напишите нам".
		/// </summary>
		public FreshAwaitCommand AskCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<WriteToUsPageModel>();
				tcs.SetResult(true);
			});
		#endregion
	}
}
