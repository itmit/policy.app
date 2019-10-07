using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;

namespace policy.app.ViewModel
{
	public class AnswerViewModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		private string _otherText;
		private readonly QuestionViewModel _question;
		#endregion
		#endregion

		#region .ctor
		public AnswerViewModel(QuestionViewModel question) => _question = question;
		#endregion

		#region Properties
		public Answer Answer
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public bool IsVisibleOtherText
		{
			get;
			set;
		}

		public ICommand SelectCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				_question.EventSelectedAnswer.Execute(this);
				tcs.SetResult(true);
			});
		#endregion
	}
}
