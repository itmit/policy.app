using System;
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
	/// Представляет модель представления для страницы профиля суслика.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class UserPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		/// <summary>
		/// Приложение.
		/// </summary>
		private App _app;

		/// <summary>
		/// Сервис для работы с api сусликов.
		/// </summary>
		private IGopherService _service;

		/// <summary>
		/// Пользователь приложения.
		/// </summary>
		private User _user;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает количество отрицательных оценок сусликов.
		/// </summary>
		public int Dislikes
		{
			get;
			set;
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
		/// Возвращает или устанавливает является ли суслик избранным.
		/// </summary>
		public bool IsFavorite
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
		/// Возвращает команду для добавление или удаления из избранного.
		/// </summary>
		public FreshAwaitCommand AddOrRemoveFavoritesCommand =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				if (_app == null || _user == null)
				{
					return;
				}

				var repository = new UserRepository(_app.RealmConfiguration);

				if (IsFavorite)
				{
					if (await _service.RemoveFromFavorites(Gopher, _user))
					{
						await _app.MainPage.DisplayAlert("Внимание", "Пользователь удален из избранного.", "ок");
						_user.FavoriteGophers.Remove((Gopher) Gopher);
						repository.Update(_user);
						IsFavorite = false;
					}
				}
				else
				{
					if (await _service.AddToFavorites(Gopher, _user))
					{
						await _app.MainPage.DisplayAlert("Внимание", "Пользователь добавлен в избранное.", "ок");
						_user.FavoriteGophers.Add((Gopher) Gopher);
						repository.Update(_user);
						IsFavorite = true;
					}
				}

				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает цвет кнопки для добавления или удаления из избранного.
		/// </summary>
		public Color FavoriteButtonColor => IsFavorite ? Color.LightSlateGray : Color.FromHex("#f07d14");

		/// <summary>
		/// Возвращает заголовок для кнопки добавления или удаления из избранного.
		/// </summary>
		public string FavoriteButtonLabel => IsFavorite ? "Убрать из избранного" : "Добавить в избранное";

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetDislike =>
			new FreshAwaitCommand((obj, tcs) =>
			{
				Dislikes++;
				Rate(RateType.Dislikes);
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для установки положительных оценок.
		/// </summary>
		public FreshAwaitCommand SetLike =>
			new FreshAwaitCommand((obj, tcs) =>
			{
				Likes++;
				Rate(RateType.Likes);
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetNeutral =>
			new FreshAwaitCommand((obj, tcs) =>
			{
				Neutrals++;
				Rate(RateType.Neutrals);
				tcs.SetResult(true);
			});
		#endregion

		#region Overrided
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
					var repository = new UserRepository(_app.RealmConfiguration);
					_user = repository.All()
									  .Single();
					_service = new GopherService(new UserToken
					{
						Token = _user.Token.Token,
						TokenType = _user.Token.TokenType
					});

					IsFavorite = _user.FavoriteGophers.Any(favoriteGopher => favoriteGopher.Guid.Equals(gopher.Guid));

					LoadGopher(gopher.Guid);
				}
			}
		}
		#endregion

		#region Private
		/// <summary>
		/// Загружает данные суслика.
		/// </summary>
		/// <param name="guid">Ид суслика.</param>
		private async void LoadGopher(Guid guid)
		{
			if (_app != null && guid != Guid.Empty)
			{
				var gopher = await _service.GetGopher(guid);
				Likes = gopher.Likes;
				Neutrals = gopher.Neutrals;
				Dislikes = gopher.Dislikes;
				Gopher = gopher;
			}
		}

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
		#endregion
	}
}
