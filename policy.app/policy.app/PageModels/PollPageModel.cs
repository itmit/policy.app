using System.Windows.Input;
using FreshMvvm;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы списка категорий опросов. 
	/// </summary>
	[AddINotifyPropertyChangedInterface]
    public class PollPageModel : FreshBasePageModel
    {
		/// <summary>
		/// Возвращает команду для открытия списка всех опросов.
		/// </summary>
		public ICommand OpenAllPollsPageCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<AllQuestionsPageModel>();
				tcs.SetResult(true);
			});


		/// <summary>
		/// Возвращает команду для открытия списка всех опросов.
		/// </summary>
		public ICommand OpenUsPollsPageCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<AllQuestionsPageModel>();
				tcs.SetResult(true);
			});
	}
}
