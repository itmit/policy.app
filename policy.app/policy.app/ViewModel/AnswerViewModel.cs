using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;

namespace policy.app.ViewModel
{
	[AddINotifyPropertyChangedInterface]
	public class AnswerViewModel : INotifyPropertyChanged
	{
		#region Data
		#region Fields
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
			get => Answer.IsSelected;
			set
			{
				if (Answer.IsSelected == value)
				{
					return;
				}

				if (!_question.Question.Multiple && value)
				{
					foreach (var answerViewModel in _question.Answers)
					{
						if (answerViewModel.Answer.IsOther)
						{
							answerViewModel.IsVisibleOtherText = false;
						}

						answerViewModel.Answer.IsSelected = false;
						answerViewModel.NotifySelectedChanged();
					}
				}

				if (Answer.IsOther)
				{
					IsVisibleOtherText = !IsVisibleOtherText;
				}

				Answer.IsSelected = value;
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelected)));
			}
		}

		public void NotifySelectedChanged()
		{
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelected)));
		}

		public bool IsVisibleOtherText
		{
			get;
			set;
		}

		public ICommand SelectCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				IsSelected = !IsSelected;
				tcs.SetResult(true);
			});
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}
