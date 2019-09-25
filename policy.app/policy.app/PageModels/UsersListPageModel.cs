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
		private Category _category;
		private App _app;
		private IGopherService _service;
		private IGopher _selectedGopher;

		public override void Init(object initData)
		{
			base.Init(initData);

			if (initData is Category category)
			{
				_category = category;
				_app = Application.Current as App;
				if (_app != null && _app.IsUserLoggedIn)
				{
					var repository = new UserRepository(_app.RealmConfiguration);
					var user = repository.All().Single();
					var token = new UserToken
					{
						Token = (string) user.Token.Token.Clone(),
						TokenType = (string) user.Token.TokenType.Clone()
					};
					_service = new GopherService(token);
					LoadGophers();
				}
			}
		}

		private async void LoadGophers()
		{
			Users = new ObservableCollection<IGopher>(await _service.GetGophers(_category));
		}

		public ObservableCollection<IGopher> Users
		{
			get;
			set;
		}

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

		public Command<IGopher> EventSelected =>
			new Command<IGopher>(obj => {
				CoreMethods.PushPageModel<UserPageModel>(obj);
			});
	}
}
