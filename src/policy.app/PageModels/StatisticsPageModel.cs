using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using policy.app.Repositories;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	public class StatisticsPageModel : FreshBasePageModel
	{
		private Guid _uuid;
		private string _selectedItem;

		public override void Init(object initData)
		{
			base.Init(initData);

			if (initData is Guid uuid)
			{
				_uuid = uuid;
				ReloadWebView(7, uuid);
			}
		}

		public string SelectedItem
		{
			get => _selectedItem;
			set
			{
				int days;
				_selectedItem = value;
				switch (value)
				{
					case "Месяц":
						days = 30;
						break;
					case "Квартал":
						days = 90;
						break;
					case "Год":
						days = 365;
						break;
					default:
						days = 7;
						break;
				}

				ReloadWebView(days, _uuid);
			}
		}

		private async void ReloadWebView(int days, Guid gopherUuid)
		{
			using (var client = new HttpClient())
			{
				var repository = new UserRepository((Application.Current as App)?.RealmConfiguration);
				var user = repository.All()
									 .Single();
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.Token.TokenType} {user.Token.Token}");

				var response = await client.GetAsync($"http://policy.itmit-studio.ru/api/statistic/{gopherUuid}/{days}");

				HtmlSource = new HtmlWebViewSource
				{
					Html = await response.Content.ReadAsStringAsync()
				};
			}
		}

		public ICommand BackModalCommand => new FreshAwaitCommand((obj, tcs) =>
		{
			CoreMethods.PopPageModel();
			tcs.SetResult(true);
		});


		public HtmlWebViewSource HtmlSource
		{
			get;
			private set;
		}
	}
}
