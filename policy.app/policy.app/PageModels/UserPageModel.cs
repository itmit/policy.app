using System;
using System.Linq;
using System.Windows.Input;
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
		private readonly App _app = App.Current;

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
		/// Представляет метод обновления профиля.
		/// </summary>
		public delegate void UpdateUserEventHandler();

		/// <summary>
		/// Происходит после обновлений данных у пользователя.
		/// </summary>
		public static event UpdateUserEventHandler UpdateUser;

		/// <summary>
		/// Провоцирует событие <see cref="UpdateUser"/>.
		/// </summary>
		public static void InvokeUpdateUser()
		{
			UpdateUser?.Invoke();
		}

		/// <summary>
		/// Обновляет данные пользователя в профиле.
		/// </summary>
		protected virtual void OnUpdateUser()
		{
			if (!IsRefreshing)
			{
				RefreshCommand.Execute(null);
			}
		}

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

				FavoritesPageModel.InvokeUpdateFavorites();
				tcs.SetResult(true);
			});


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
			new FreshAwaitCommand((obj, tcs) =>
			{
				IsRefreshing = true;
				LoadGopher(Gopher.Guid);
				IsRefreshing = false;
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
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				Dislikes++;
				var res = await _service.Rate(Gopher, RateType.Dislikes);
				if (res)
				{
					UpdateUser?.Invoke();
				}
				else
				{
					Dislikes--;
				}
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для установки положительных оценок.
		/// </summary>
		public FreshAwaitCommand SetLike =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				Likes++;
				var res = await _service.Rate(Gopher, RateType.Likes);
				if (res)
				{
					UpdateUser?.Invoke();
				}
				else
				{
					Likes--;
				}
				tcs.SetResult(true);
			});

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetNeutral =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				Neutrals++;
				var res = await _service.Rate(Gopher, RateType.Neutrals);
				if (res)
				{
					UpdateUser?.Invoke();
				}
				else
				{
					Neutrals--;
				}
				tcs.SetResult(res);
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
			UpdateUser += OnUpdateUser;

			if (initData is IGopher gopher)
			{
				Gopher = gopher;

				if (_app != null)
				{
					var repository = new UserRepository(_app.RealmConfiguration);
					_user = repository.All()
									  .Single();
					_service = new GopherService(_user.Token);

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
			IsFavorite = _user.FavoriteGophers.Any(favoriteGopher => favoriteGopher.Guid.Equals(guid));
			var gopher = await _service.GetGopher(guid);
			if (string.IsNullOrEmpty(gopher.PhotoSource))
			{
				gopher.PhotoSource = "def_profile";
			}
			Likes = gopher.Likes;
			Neutrals = gopher.Neutrals;
			Dislikes = gopher.Dislikes;
			Gopher = gopher;
		}
		#endregion
	}
}
