using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;
using static System.String;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы "Категории".
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class CategoriesPageModel : FreshBasePageModel
	{
		/// <summary>
		/// Сервис для загрузки категорий.
		/// </summary>
		private GopherService _service;

		/// <summary>
		/// Текущий <see cref="Application"/>.
		/// </summary>
		private App _app;
		private Category _selectedCategory;

        /// <summary>
        /// Вызывается при загрузке модели представления, загружает список категорий.
        /// </summary>
        /// <param name="initData"></param>
        public override void Init(object initData)
		{
			base.Init(initData);

			if (Application.Current is App app)
			{
				_app = app;

				if (app.IsUserLoggedIn)
				{
					var repository = new UserRepository(_app.RealmConfiguration);
					var token = repository.All().SingleOrDefault()?.Token;

					if (token != null && !IsNullOrEmpty(token.Token))
					{
						_service = new GopherService(new UserToken
						{
							Token = (string)token.Token.Clone(),
							TokenType = (string)token.TokenType.Clone()
						});

						LoadCategories();
					}
				}
			}
		}

		/// <summary>
		/// Возвращает или устанавливает выбранную категорию.
		/// </summary>
		public Category SelectedCategory
		{
			get => _selectedCategory;
			set
			{
				_selectedCategory = value;
				if (value != null)
				{
					EventSelected.Execute(value);
				}
			}
		}

        /// <summary>
        /// Загружает категории при помощи сервиса.
        /// </summary>
        private async void LoadCategories()
		{
			if (_service != null)
			{
				Categories = new ObservableCollection<Category>(await _service.GetCategories());
            }
		}

		public FreshAwaitCommand OpenVotes =>
			new FreshAwaitCommand((param, tcs) =>
			{
				CoreMethods.PushPageModel<PollPageModel>();
				tcs.SetResult(true);
			});


		/// <summary>
		/// Возвращает или устанавливает категории.
		/// </summary>
		public ObservableCollection<Category> Categories
		{
			get;
			set;
		} = new ObservableCollection<Category>();

		public Command<Category> EventSelected =>
			new Command<Category>(obj => {
				CoreMethods.PushPageModel<UsersListPageModel>(obj);
			});
	}
}
