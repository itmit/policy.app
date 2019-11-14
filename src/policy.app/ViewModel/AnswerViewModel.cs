using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;

namespace policy.app.ViewModel
{
	/// <summary>
	/// Представляет модель представления для строки с ответом.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class AnswerViewModel : INotifyPropertyChanged
	{
		#region Data
		#region Fields
		/// <summary>
		/// Родительская модель представления для вопроса.
		/// </summary>
		private readonly QuestionViewModel _question;
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует <see cref="AnswerViewModel"/>, с родительской моделью представления.
		/// </summary>
		/// <param name="question">Родительская модель представления, вопроса.</param>
		public AnswerViewModel(QuestionViewModel question) => _question = question;
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает модель вопроса.
		/// </summary>
		public Answer Answer
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает выбран ли ответ.
		/// </summary>
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

		/// <summary>
		/// Уведомляет представление об изменении выбранного ответа.
		/// </summary>
		public void NotifySelectedChanged()
		{
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelected)));
		}

		/// <summary>
		/// Возвращает или устанавливает текст другого варианта ответа.
		/// </summary>
		public bool IsVisibleOtherText
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает команду для выбора варианта ответа.
		/// </summary>
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
