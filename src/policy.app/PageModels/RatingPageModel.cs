using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using policy.app.ViewModel;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы рейтинга.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class RatingPageModel : BaseMainPageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Текущее приложение.
		/// </summary>
		private readonly App _app = App.Current;
		
		/// <summary>
		/// Сервис для работы с сусликами.
		/// </summary>
		private IGopherService _service;

		/// <summary>
		/// Выбранная категория.
		/// </summary>
		private Category _selectedCategory;

		/// <summary>
		/// Поисковый запрос.
		/// </summary>
		private string _query;

		/// <summary>
		/// Выбранная сортировка.
		/// </summary>
		private string _selectedSort;

		/// <summary>
		/// Тип сортировки.
		/// </summary>
		private string _sort = "desc";

		/// <summary>
		/// Репозиторий для работы с данными пользователя.
		/// </summary>
		private UserRepository _repository;
		private IGopher _selectedGopher;
		#endregion
		#endregion

		/// <summary>
		/// Инициализирует модель представления для домашней страницы.
		/// </summary>
		/// <param name="initData">Параметр модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);


			_repository = new UserRepository(_app.RealmConfiguration);
			var token = _repository.All()
								  .SingleOrDefault();
			if (token != null)
			{
				_service = new GopherService(token.Token);
			}
		}

		public Category SelectedCategory
		{
			get => _selectedCategory;
			set
			{
				_selectedCategory = value;

				if (value != null)
				{
					Task.Run(LoadGophers);
				}
			}
		}

		public string SelectedSort
		{
			get => _selectedSort;
			set
			{
				_selectedSort = value;
				if (value == "От лучшего к худшему")
				{
					_sort = "desc";
				}
				else if (value == "От худшего к лучшему")
				{
					_sort = "asc";
				}
				Task.Run(LoadGophers);
			}
		}

		/// <summary>
		/// Возвращает или устанавливает сусликов в избранном.
		/// </summary>
		public ObservableCollection<IGopher> Gophers
		{
			get;
			set;
		} = new ObservableCollection<IGopher>();

		public ObservableCollection<IGopher> AllGophers
		{
			get;
			set;
		} = new ObservableCollection<IGopher>();

		/// <summary>
		/// Загружает опросы.
		/// </summary>
		private async void LoadGophers()
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet || _service == null)
			{
				return;
			}

			var repository = new UserRepository(_app.RealmConfiguration);
			var user = repository.All()
								 .Single();
			AllGophers = new ObservableCollection<IGopher>(await _service.Search(_sort, _query, _selectedCategory));
			Gophers.Clear();
			PageNumber = -1;
			Device.BeginInvokeOnMainThread(MoveNext);
			repository.Update(user);
		}

		public IGopher SelectedGopher
		{
			get => _selectedGopher;
			set
			{
				if (value == null)
				{
					return;
				}

				_selectedGopher = value;
				RaisePropertyChanged(nameof(SelectedGopher));

				CoreMethods.PushPageModel<UserPageModel>(value);

				_selectedGopher = null;
				RaisePropertyChanged(nameof(SelectedGopher));

			}
		}

		/// <summary>
		/// Возвращает или устанавливает перезагружается ли список избранных сусликов.
		/// </summary>
		public bool IsRefreshing
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает команду для обновления списка избранных. 
		/// </summary>
		public ICommand RefreshCommand =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				IsRefreshing = true;

				if (_service == null)
				{
					return;
				}

				await Task.Run(LoadGophers);
				Categories = new ObservableCollection<Category>(await _service.GetCategories());

				IsRefreshing = false;
				tcs.SetResult(true);
			});

		public string Query
		{
			get => _query;
			set
			{
				_query = value;
				if (value != null)
				{
					Task.Run(LoadGophers);
				}
			}
		}

		/// <summary>
		/// Возвращает или устанавливает список категорий.
		/// </summary>
		public ObservableCollection<Category> Categories
		{
			get;
			set;
		}

		public override void LoadData()
		{
			Task.Run(async () =>
			{
				LoadGophers();
				Categories = new ObservableCollection<Category>(await _service.GetCategories());
			});
			
		}

		public void MoveNext()
		{
			IsBusy = true;
			PageNumber++;

			var offset = PageNumber * PageSize;
			if (offset >= AllGophers.Count)
			{
				IsBusy = false;
				return;
			}

			var limit = PageSize;
			limit = AllGophers.Count - offset < limit ? AllGophers.Count - offset : limit;
			for (int i = offset; i < offset + limit; i++)
			{
				Gophers.Add(AllGophers[i]);
			}
			RaisePropertyChanged(nameof(Gophers));
			IsBusy = false;
		}

		public int PageSize
		{
			get;
			set;
		} = 50;

		public int PageNumber
		{
			get;
			set;
		} = -1;

		public bool IsBusy
		{
			get;
			set;
		}
	}
}
