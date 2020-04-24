using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы списка опросов.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class AllQuestionsPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;
		private Poll _selectedPoll;
		private IPollService _service;
		private PollCategory _pollCategory;
		private PollCategory _selectedPollCategory;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает список опросов.
		/// </summary>
		public ObservableCollection<Poll> Polls
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает выбранный опрос.
		/// </summary>
		public Poll SelectedPoll
		{
			get => _selectedPoll;
			set
			{
				if (value == null)
				{
					return;
				}

				_selectedPoll = value;
				RaisePropertyChanged(nameof(SelectedPoll));

				CoreMethods.PushPageModel<SurveyPageModel>(value);

				_selectedPoll = null;
				RaisePropertyChanged(nameof(SelectedPoll));
			}
		}
		#endregion

		public string Title
		{
			get;
			set;
		} = "Все опросы.";

		#region Overrided
		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (initData is PollCategory pollCategory)
			{
				_pollCategory = pollCategory;
				Title = _pollCategory.Name;
			}

			if (_app == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .SingleOrDefault();

			if (user != null)
			{
				try
				{
					_service = new PollService(new UserToken
											   {
												   Token = user.Token.Token,
												   TokenType = user.Token.TokenType
											   });
					LoadPolls();
					LoadPollCategories();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}
		#endregion

		#region Private
		private async void LoadPollCategories()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var polls = new ObservableCollection<PollCategory>(await _service.GetPollCategories(_pollCategory.Guid));
			foreach (var pollCategory in polls)
			{
				pollCategory.ImageSource = "OurPoll";
			}
			PollCategories = polls;
		}


		/// <summary>
		/// Возвращает или устанавливает выбранную категорию опросов.
		/// </summary>
		public PollCategory SelectedPollCategory
		{
			get => _selectedPollCategory;
			set
			{
				_selectedPollCategory = value;

				if (value != null)
				{
					EventSelected1.Execute(value);
				}
			}
		}

		/// <summary>
		/// Возвращает или устанавливает команду при выборе опроса.
		/// </summary>
		public Command<PollCategory> EventSelected1 =>
			new Command<PollCategory>(obj =>
			{
				if (obj is PollCategory poll)
				{
					CoreMethods.PushPageModel<AllQuestionsPageModel>(poll);
				}
			});

		public ObservableCollection<PollCategory> PollCategories
		{
			get;
			set;
		}

		/// <summary>
		/// Загружает опросы.
		/// </summary>
		private async void LoadPolls()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var polls = new ObservableCollection<Poll>(await _service.GetPolls(_pollCategory.Guid));
			for (var i = 0; i < polls.Count; i++)
			{
				polls[i]
					.PollListNumber = i + 1;
			}

			Polls = polls;
		}
		#endregion
	}
}
