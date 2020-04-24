using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
	/// Представляет модель представления для страницы списка категорий опросов.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class PollPageModel : FreshBasePageModel
	{

		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;
		private IPollService _service;
		private PollCategory _selectedPollCategory;

		#region Properties
		/// <summary>
		/// Возвращает команду для открытия списка всех опросов.
		/// </summary>
		public ICommand OpenAllPollsPageCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<AllQuestionsPageModel>();
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для открытия списка всех опросов.
		/// </summary>
		public ICommand OpenUsPollsPageCommand =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<AllQuestionsPageModel>();
				tcs.SetResult(true);
			});
		#endregion

		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (_app == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .SingleOrDefault();

			if (user != null)
			{
				_service = new PollService(new UserToken
										   {
											   Token = user.Token.Token,
											   TokenType = user.Token.TokenType
										   });
				LoadPollCategories();
			}
		}

		private async void LoadPollCategories()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var polls = new ObservableCollection<PollCategory>(await _service.GetPollCategories());
			foreach (var pollCategory in polls)
			{
				pollCategory.ImageSource = "OurPoll";
			}
			polls.Insert(0, new PollCategory
			{
				Guid = Guid.Empty,
				Name = "Все опросы",
				ImageSource = "AllPoll"
			});
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
				if (value == null)
				{
					return;
				}

				_selectedPollCategory = value;
				RaisePropertyChanged(nameof(SelectedPollCategory));

				CoreMethods.PushPageModel<AllQuestionsPageModel>(value);

				_selectedPollCategory = null;
				RaisePropertyChanged(nameof(SelectedPollCategory));

			}
		}

		public ObservableCollection<PollCategory> PollCategories
		{
			get;
			set;
		}
	}
}
