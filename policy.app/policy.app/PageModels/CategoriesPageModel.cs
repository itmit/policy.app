using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
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

		/// <summary>
		/// Вызывается при загрузке модели представления, загружает список категорий.
		/// </summary>
		/// <param name="initData"></param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (Application.Current is App app)
			{
				if (app.IsUserLoggedIn)
				{
					_app = app;

					var token = _app.Realm.All<User>().Single().Token;

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
		/// Загружает категории при помощи сервиса.
		/// </summary>
		private async void LoadCategories()
		{
			Categories = new ObservableCollection<Category>(await _service.GetCategories());
		}

		/// <summary>
		/// Возвращает или устанавливает категории.
		/// </summary>
		public ObservableCollection<Category> Categories
		{
			get;
			set;
		} = new ObservableCollection<Category>();
	}
}
