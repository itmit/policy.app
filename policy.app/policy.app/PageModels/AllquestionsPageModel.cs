using System.Windows.Input;
using FreshMvvm;
using PropertyChanged;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы списка опросов. 
	/// </summary>
    [AddINotifyPropertyChangedInterface]
    public class AllQuestionsPageModel : FreshBasePageModel
    {
		/// <summary>
		/// Возвращает команду для открытия опроса.
		/// </summary>
		public ICommand OpenSurveyPage
		{
			get => new FreshAwaitCommand((param, tcs) =>
				{
					CoreMethods.PushPageModel<SurveyPageModel>();
					tcs.SetResult(true);
				});
			
		}
	}
}
