using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;

namespace policy.app.ViewModel
{
	public class AnswerViewModel : FreshBasePageModel
	{
		private QuestionViewModel _question;
		private string _otherText;

		public AnswerViewModel(QuestionViewModel question) => _question = question;

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
	}
}
