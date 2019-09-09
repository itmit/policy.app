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
		private App _app;
		private GopherService _service;

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

		public IGopher Gopher
		{
			get;
			set;
		}

		public int Likes
		{
			get;
			set;
		}

		public int Neutrals
		{
			get;
			set;
		}

		public int Dislikes
		{
			get;
			set;
		}

		public FreshAwaitCommand SetLike => new FreshAwaitCommand((obj, tcs) =>
		{
			Likes++;
			Rate(RateType.Likes);
			tcs.SetResult(true);
		});
		public FreshAwaitCommand SetNeutral => new FreshAwaitCommand((obj, tcs) =>
		{
			Neutrals++;
			Rate(RateType.Neutrals);
			tcs.SetResult(true);
		});

		public FreshAwaitCommand SetDislike => new FreshAwaitCommand((obj, tcs) =>
		{
			Dislikes++;
			Rate(RateType.Dislikes);
			tcs.SetResult(true);
		});

		private async void Rate(RateType rateType)
		{
			if (_service != null)
			{
				await _service.Rate(Gopher, rateType);
			}
		}
	}
}
