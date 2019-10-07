﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using Xamarin.Forms;

namespace policy.app.ViewModel
{
	/// <summary>
	/// Представляет модель представления для вопроса.
	/// </summary>
	public class QuestionViewModel : FreshBasePageModel
	{
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

		/// <summary>
		/// Возвращает или устанавливает моделей ответов.
		/// </summary>
		public ObservableCollection<AnswerViewModel> Answers
		{
			get;
			set;
		} = new ObservableCollection<AnswerViewModel>();

		/// <summary>
		/// Возвращает или устанавливает команду при выборе ответа.
		/// </summary>
		public Command<AnswerViewModel> EventSelectedAnswer =>
			new Command<AnswerViewModel>(obj => {
				if (obj is AnswerViewModel answer)
				{
					if (!Question.Multiple && !answer.IsSelected)
					{
						foreach (var answerViewModel in Answers)
						{
							answerViewModel.IsSelected = false;
							answerViewModel.Answer.IsSelected = false;
						}
					}

					answer.IsSelected = !answer.IsSelected;
					answer.Answer.IsSelected = answer.IsSelected;
				}
			});
	}
}
