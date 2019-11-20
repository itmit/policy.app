﻿using System.Collections.ObjectModel;
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
		/// Возвращает или устанавливает команду при выборе опроса.
		/// </summary>
		public Command<Poll> EventSelected =>
			new Command<Poll>(obj =>
			{
				if (obj is Poll poll)
				{
					CoreMethods.PushPageModel<SurveyPageModel>(poll);
				}
			});

		/// <summary>
		/// Возвращает или устанавливает выбранный опрос.
		/// </summary>
		public Poll SelectedPoll
		{
			get => _selectedPoll;
			set
			{
				_selectedPoll = value;

				if (value != null)
				{
					EventSelected.Execute(value);
				}
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
				_service = new PollService(new UserToken
										   {
											   Token = user.Token.Token,
											   TokenType = user.Token.TokenType
										   },
										   new HttpClient());
				LoadPolls();
			}
		}
		#endregion

		#region Private
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