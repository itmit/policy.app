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

		/// <summary>
		/// Возвращает или устанавливает команду при выборе суслика.
		/// </summary>
		public Command<IGopher> EventSelectedGopher =>
			new Command<IGopher>(obj =>
			{
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});

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
		public ObservableCollection<SearchGopherViewModel> Gophers
		{
			get;
			set;
		} = new ObservableCollection<SearchGopherViewModel>();

		public ObservableCollection<SearchGopherViewModel> AllGophers
		{
			get;
			set;
		} = new ObservableCollection<SearchGopherViewModel>();

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
			var gophers = new ObservableCollection<IGopher>(await _service.Search(_sort, _query, _selectedCategory));
			var vmGophers = new ObservableCollection<SearchGopherViewModel>();
			foreach (var gopher in gophers)
			{
				vmGophers.Add(new SearchGopherViewModel(gopher, this));
			}

			AllGophers = vmGophers;
			PageNumber = -1;
			MoveNext();
			repository.Update(user);
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
			PageNumber++;

			var offset = PageNumber * PageSize;
			if (offset >= AllGophers.Count)
			{
				return;
			}

			var limit = PageSize;
			limit = AllGophers.Count - offset < limit ? Gophers.Count - offset : limit;

			var a = AllGophers.ToList().GetRange(offset, limit);
			var list = Gophers.ToList();
			list.AddRange(a);
			Gophers = new ObservableCollection<SearchGopherViewModel>(list);
			IsRefreshing = false;
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
	}
}
