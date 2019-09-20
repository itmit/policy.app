using System;
using System.Linq;
using FreshMvvm;
using policy.app.Models;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для страницы профиля суслика.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class UserPageModel : FreshBasePageModel
	{
		/// <summary>
		/// Приложение.
		/// </summary>
		private App _app;

		/// <summary>
		/// Сервис для работы с api сусликов.
		/// </summary>
		private GopherService _service;

		/// <summary>
		/// Инициализирует модель представления.
		/// </summary>
		/// <param name="initData">Параметры модели представления.</param>
		public override void Init(object initData)
		{
			base.Init(initData);

			if (initData is IGopher gopher)
			{
				Gopher = gopher;
			
				_app = Application.Current as App;
				if (_app != null && _app.IsUserLoggedIn)
				{
					var token = _app.Realm.All<User>()
									.Single();
					_service = new GopherService(new UserToken
					{
						Token = (string)token.Token.Token.Clone(),
						TokenType = (string)token.Token.TokenType.Clone()
					});

					LoadGopher(Guid.Parse(gopher.Guid));
				}
			}
		}

		/// <summary>
		/// Загружает данные суслика.
		/// </summary>
		/// <param name="guid">Ид суслика.</param>
		private async void LoadGopher(Guid guid)
		{
			if (_app != null && guid != Guid.Empty)
			{
				var gopher = await _service.GetGopher(guid);
				gopher.Category = Gopher.Category;
				Likes = gopher.Likes;
				Neutrals = gopher.Neutrals;
				Dislikes = gopher.Dislikes;
				Gopher = gopher;
			}
		}

		/// <summary>
		/// Возвращает или устанавливает суслика.
		/// </summary>
		public IGopher Gopher
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает количество оценок "Нравится" сусликов.
		/// </summary>
		public int Likes
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает количество нейтральных оценок сусликов.
		/// </summary>
		public int Neutrals
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает количество отрицательных оценок сусликов.
		/// </summary>
		public int Dislikes
		{
			get;
			set;
		}

		public FreshAwaitCommand AddToFavoritesCommand => new FreshAwaitCommand(async (obj, tcs) =>
		{
			if (_app == null)
			{
				return;
			}

			using (var realm = _app.Realm)
			{
				var user = realm.All<User>()?.SingleOrDefault();

				if (user != null)
				{
					if (await _service.AddToFavorites(Gopher, user))
					{
						await _app.MainPage.DisplayAlert("Внимание", "Пользователь добавлен в избранное.", "ок");
					}
				}
			}
			tcs.SetResult(true);
		});

		/// <summary>
		/// Возвращает команду для установки положительных оценок.
		/// </summary>
		public FreshAwaitCommand SetLike => new FreshAwaitCommand((obj, tcs) =>
		{
			Likes++;
			Rate(RateType.Likes);
			tcs.SetResult(true);
		});

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetNeutral => new FreshAwaitCommand((obj, tcs) =>
		{
			Neutrals++;
			Rate(RateType.Neutrals);
			tcs.SetResult(true);
		});

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetDislike => new FreshAwaitCommand((obj, tcs) =>
		{
			Dislikes++;
			Rate(RateType.Dislikes);
			tcs.SetResult(true);
		});

		/// <summary>
		/// Устанавливает оценку.
		/// </summary>
		/// <param name="rateType">Тип оценки.</param>
		private async void Rate(RateType rateType)
		{
			if (_service != null)
			{
				await _service.Rate(Gopher, rateType);
			}
		}
	}
}
