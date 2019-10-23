using System.Collections.ObjectModel;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.ViewModel
{
	/// <summary>
	/// Представляет модель представления для вопроса.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class QuestionViewModel
	{
		#region .ctor
		public QuestionViewModel(Question question)
		{
			Question = question;
			foreach (var answer in Question.Answers)
			{
				Answers.Add(new AnswerViewModel(this)
				{
					Answer = answer
				});
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает моделей ответов.
		/// </summary>
		public ObservableCollection<AnswerViewModel> Answers
		{
			get;
			set;
		} = new ObservableCollection<AnswerViewModel>();

		/// <summary>
		/// Возвращает или устанавливает номер вопроса в списке.
		/// </summary>
		public int ListNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает вопрос.
		/// </summary>
		public Question Question
		{
			get;
			set;
		}
		#endregion
	}
}
