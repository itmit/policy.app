﻿using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using policy.app.ViewModel;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class SurveyPageModel : FreshBasePageModel
    {
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = Application.Current as App;

		/// <summary>
		/// Сервис для работы с опросом.
		/// </summary>
		private IPollService _service;

		/// <summary>
		/// Опрос.
		/// </summary>
		private Poll _poll;

		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (_app == null || !_app.IsUserLoggedIn)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All().Single();
			_service = new PollService(new UserToken
			{
				Token = user.Token.Token,
				TokenType = user.Token.TokenType
			});

			if (initData is Poll poll)
			{
				_poll = poll;
				LoadQuestions();
			}
		}

		/// <summary>
		/// Загружает вопросы.
		/// </summary>
		public async void LoadQuestions()
		{
			var questions = new ObservableCollection<Question>(await _service.GetQuestions(_poll.Guid));
			var questionsViewModels = new ObservableCollection<QuestionViewModel>();
			for (int i = 0; i < questions.Count; i++)
			{
				questionsViewModels.Add(new QuestionViewModel(questions[i])
					{
						ListNumber = i + 1
					});
			}
			Questions = questionsViewModels;

		}

		/// <summary>
		/// Возвращает или устанавливает вопросы.
		/// </summary>
		public ObservableCollection<QuestionViewModel> Questions
		{
			get;
			set;
		}
	}
}
