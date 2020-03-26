using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using policy.app.Services;
using PropertyChanged;
using Xamarin.Essentials;
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
		private bool _isVoted = true;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Представляет метод обновления профиля.
		/// </summary>
		public delegate void UpdateUserEventHandler();

		public string FavImageSource => IsFavorite ? "icons8_star_100" : "Star_def";

		public ICommand OpenBrowserCommand => new FreshAwaitCommand(async (obj, tcs) =>
			{
				if (Uri.IsWellFormedUriString(Gopher.Link, UriKind.Absolute))
				{
					await Browser.OpenAsync(Gopher.Link);
					tcs.SetResult(true);
					return;
				}

				await _app.MainPage.DisplayAlert("Внимание", "Неправильный формат ссылки.", "Ок");
				tcs.SetResult(false);
			});

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
		/// Возвращает команду для открытия статистики.
		/// </summary>
		public ICommand OpenStatisticsCommand 
			=> new FreshAwaitCommand(async (obj, tcs) =>
			{
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_user.Token.TokenType} {_user.Token.Token}");

					var response = await client.GetAsync($"http://policy.itmit-studio.ru/api/statistic/{Gopher.Guid}");

					var html = await response.Content.ReadAsStringAsync();

					var page = new ContentPage
					{
						Title = "Статистика"
					};

					var view = new WebView
					{
						Source = new HtmlWebViewSource
						{
							Html = html
						}
					};


					var top = 0;
					if (Device.iOS == Device.RuntimePlatform)
					{
						top = 50;
					}

					var imageButton = new ImageButton
					{
						BackgroundColor = Color.Transparent,
						VerticalOptions = LayoutOptions.Start,
						HorizontalOptions = LayoutOptions.Start,
						Margin = new Thickness(10, top, 0, 0),
						Source = "ic_arrow_back_ios.png",
						HeightRequest = 35,
						WidthRequest = 35,
						Command = BackModalCommand
					};

					var grid = new Grid();
					grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
					grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
					grid.Children.Add(imageButton, 0, 1);
					grid.Children.Add(view, 0, 2);
					page.Content = grid;

					await Application.Current.MainPage.Navigation.PushModalAsync(page);
					tcs.SetResult(true);
				}
			});


		public ICommand BackModalCommand => new FreshAwaitCommand((obj, tcs) =>
		{
			tcs.SetResult(true);

			Application.Current.MainPage.Navigation.PopModalAsync(false);
		});

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

		public string Link
		{
			get;
			private set;
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
				DislikesImageSource = "dislike";
				Dislikes++;
				var res = await _service.Rate(Gopher, RateType.Dislikes);
				if (res)
				{
					_isVoted = false;
					SetDislike.CanExecute(null);
					SetLike.CanExecute(null);
					SetNeutral.CanExecute(null);
					UpdateUser?.Invoke();
				}
				else
				{
					DislikesImageSource = "img_1315401";
					Dislikes--;
				}
				tcs.SetResult(true);
			}, obj => _isVoted);

		/// <summary>
		/// Возвращает команду для установки положительных оценок.
		/// </summary>
		public FreshAwaitCommand SetLike =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				LikeImageSource = "like";
				Likes++;
				var res = await _service.Rate(Gopher, RateType.Likes);
				if (res)
				{
					_isVoted = false;
					SetDislike.CanExecute(null);
					SetLike.CanExecute(null);
					SetNeutral.CanExecute(null);
					UpdateUser?.Invoke();
				}
				else
				{
					LikeImageSource = "img_131540";
					Likes--;
				}
				tcs.SetResult(true);
			}, obj => _isVoted);

		/// <summary>
		/// Возвращает команду для установки нейтральных оценок.
		/// </summary>
		public FreshAwaitCommand SetNeutral =>
			new FreshAwaitCommand(async (obj, tcs) =>
			{
				NeutralsImageSource = "neutral";
				Neutrals++;
				var res = await _service.Rate(Gopher, RateType.Neutrals);
				if (res)
				{
					_isVoted = false;
					SetDislike.CanExecute(null);
					SetLike.CanExecute(null);
					SetNeutral.CanExecute(null);
					UpdateUser?.Invoke();
				}
				else
				{
					NeutralsImageSource = "meh";
					Neutrals--;
				}
				tcs.SetResult(res);
			}, obj => _isVoted);
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

		public string LikeImageSource
		{
			get;
			set;
		} = "img_131540";

		public string NeutralsImageSource
		{
			get;
			set;
		} = "meh";

		public string DislikesImageSource
		{
			get;
			set;
		} = "img_1315401";

		#region Private
		/// <summary>
		/// Загружает данные суслика.
		/// </summary>
		/// <param name="guid">Ид суслика.</param>
		private async void LoadGopher(Guid guid)
		{
			IsFavorite = _user.FavoriteGophers.Any(favoriteGopher => favoriteGopher.Guid.Equals(guid));
			var gopher = await _service.GetGopher(guid);
			if (!string.IsNullOrEmpty(gopher.Link))
			{
				Link = "Дополнительная информация>>";
			}
			Likes = gopher.Likes;
			Neutrals = gopher.Neutrals;
			Dislikes = gopher.Dislikes;
			Gopher = gopher;

			if (gopher.Mark == null)
			{
				return;
			}

			if (gopher.Mark.Equals("likes"))
			{
				LikeImageSource = "like";
			}
			if (gopher.Mark.Equals("neutrals"))
			{
				NeutralsImageSource = "neutral";
			}
			if (gopher.Mark.Equals("dislikes"))
			{
				DislikesImageSource = "dislike";
			}
		}
		#endregion
	}
}
