using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы списка сусликов в категории.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class UsersListPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		private App _app;
		private Category _category;
		private IGopher _selectedGopher;
		private IGopherService _service;

		public List<IGopher> AllUsers
		{
			get;
			private set;
		}

		private string _query = "";
		#endregion
		#endregion

		#region Properties
		public string Query
		{
			get => _query;
			set
			{
				if (value == null || value.Equals(Query))
				{
					return;
				}
				_query = value.ToLower();
				Users.Clear();
				Page = -1;
				MoveNext();
			}
		}

		/// <summary>
		/// Возвращает или устанавливает заголовок страницы.
		/// </summary>
		public string Title
		{
			get;
			set;
		}

		public ObservableCollection<IGopher> Users
		{
			get;
			set;
		} = new ObservableCollection<IGopher>();

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

		public ObservableCollection<Category> Categories { get; private set; }

		public bool IsBusy
		{
			get;
			set;
		}
		#endregion

		#region Overrided
		public override async void Init(object initData)
		{
			base.Init(initData);

			if (initData is Category category)
			{
				_category = category;
				Title = _category.Title;
				_app = Application.Current as App;
				if (_app != null)
				{
					var repository = new UserRepository(_app.RealmConfiguration);
					var user = repository.All()
										 .Single();
					_service = new GopherService(user.Token);
					LoadGophers();
					Categories = new ObservableCollection<Category>(await _service.GetCategories(_category.Uuid));
				}
			}
		}
		#endregion

		public int Page
		{
			get;
			set;
		} = -1;

		public int PageSize
		{
			get;
			set;
		} = 15;

		#region Private
		private async void LoadGophers()
		{
			IsBusy = true;
			AllUsers = new List<IGopher>((await _service.GetGophers(_category)).OrderByDescending(o => o.Name));
			Page = -1;
			MoveNext();
		}
		#endregion

		public void MoveNext()
		{
			IsBusy = true;
			Page++;

			if (string.IsNullOrWhiteSpace(Query))
			{
				var offset = Page * PageSize;
				if (offset >= AllUsers.Count)
				{
					IsBusy = false;
					return;
				}

				var limit = PageSize;
				limit = AllUsers.Count - offset < limit ? AllUsers.Count - offset : limit;
				for (int i = offset; i < offset + limit; i++)
				{
					Users.Add(AllUsers[i]);
				}
			}
			else
			{
				var array = AllUsers.Where(user => user.Name.ToLower()
													   .Contains(Query)).ToArray();
				var offset = Page * PageSize;
				if (offset >= array.Length)
				{
					IsBusy = false;
					return;
				}

				var limit = PageSize;
				limit = array.Length - offset < limit ? array.Length - offset : limit;
				for (int i = offset; i < offset + limit; i++)
				{
					Users.Add(array[i]);
				}
			}
			RaisePropertyChanged(nameof(Users));
			IsBusy = false;
		}

		internal void OpenCategory(Category category)
		{
			CoreMethods.PushPageModel<UsersListPageModel>(category);
		}
	}
}
