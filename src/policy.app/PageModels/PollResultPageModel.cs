using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Repositories;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	public class PollResultPageModel : FreshBasePageModel
	{
		private Guid _pollGuid;
		private const string  PollResultUri = "http://policy.itmit-studio.ru/api/showPollResults/";

		public ICommand OpenResultsCommand => new FreshAwaitCommand(async (obj, tcs) =>
		{
			tcs.SetResult(true);

			using (var client = new HttpClient())
			{
				var repository = new UserRepository(App.Current.RealmConfiguration);
				var user = repository.All().Single();
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.Token.TokenType} {user.Token.Token}");

				var response = await client.GetAsync(PollResultUri + _pollGuid);

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
				page.Content = view;

				await Application.Current.MainPage.Navigation.PushModalAsync(page);
			}
		});

		public override void Init(object initData)
		{
			base.Init(initData);
			if (initData is Guid guid)
			{
				_pollGuid = guid;
			}
		}

		public ICommand BackCommand => new FreshAwaitCommand((obj, tcs) =>
		{
			tcs.SetResult(true);

			CoreMethods.PopToRoot(true);
		});
	}
}
