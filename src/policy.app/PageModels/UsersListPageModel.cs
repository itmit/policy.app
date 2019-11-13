using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		private List<IGopher> _allUsers;
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
				Users = new ObservableCollection<IGopher>(_allUsers.Where(user => user.Name.ToLower().Contains(_query)));
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
		}

		public Command<IGopher> EventSelected =>
			new Command<IGopher>(obj =>
			{
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});

		public IGopher SelectedGopher
		{
			get => _selectedGopher;
			set
			{
				_selectedGopher = value;

				if (value != null)
				{
					EventSelected.Execute(value);
				}
			}
		}
		#endregion

		#region Overrided
		public override void Init(object initData)
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
				}
			}
		}
		#endregion

		#region Private
		private async void LoadGophers()
		{
			_allUsers = new List<IGopher>(await _service.GetGophers(_category));
			Users = new ObservableCollection<IGopher>(_allUsers.Where(user => user.Name.ToLower().Contains(Query)));
		}
		#endregion
	}
}
