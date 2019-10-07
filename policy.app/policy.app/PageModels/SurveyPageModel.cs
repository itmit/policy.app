using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using policy.app.ViewModel;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы опроса. 
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class SurveyPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;

		/// <summary>
		/// Опрос.
		/// </summary>
		private Poll _poll;

		/// <summary>
		/// Сервис для работы с опросом.
		/// </summary>
		private IPollService _service;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает вопросы.
		/// </summary>
		public ObservableCollection<QuestionViewModel> Questions
		{
			get;
			set;
		}

		public ICommand PassPollCommand =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				var repository = new UserRepository(_app.RealmConfiguration);
				var users = repository.All();
				var user = users.SingleOrDefault();

				await _app.MainPage.DisplayAlert("Уведомление", "Опрос пройден.", "ОK");
				await CoreMethods.PopPageModel();

				tcs.SetResult(false);
			});
		#endregion

		#region Public
		/// <summary>
		/// Загружает вопросы.
		/// </summary>
		public async void LoadQuestions()
		{
			_poll.Questions = (await _service.GetQuestions(_poll.Guid)).ToList();
			var questionsViewModels = new ObservableCollection<QuestionViewModel>();
			for (var i = 0; i < _poll.Questions.Count; i++)
			{
				questionsViewModels.Add(new QuestionViewModel(_poll.Questions[i])
				{
					ListNumber = i + 1
				});
			}
			Questions = questionsViewModels;
		}
		#endregion

		#region Overrided
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
			var user = repository.All()
								 .Single();
			_service = new PollService(new UserToken
									   {
										   Token = user.Token.Token,
										   TokenType = user.Token.TokenType
									   },
									   new HttpClient());

			if (initData is Poll poll)
			{
				_poll = poll;
				LoadQuestions();
			}
		}
		#endregion
	}
}
