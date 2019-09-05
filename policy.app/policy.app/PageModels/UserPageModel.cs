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
				Gopher = gopher;
			}
		}

		public IGopher Gopher
		{
			get;
			set;
		}
	}
}
