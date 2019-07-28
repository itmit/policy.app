using System.Windows.Input;
using FreshMvvm;
using policy.app.Models;
using PropertyChanged;
//using Realms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class LoginPageModel : FreshBasePageModel
	{
		public string MessageLabel { get; set; } = string.Empty;

		// public Realm Realm { get { return Realm.GetInstance(); } }

		public string UsernameEntry { get; set; } = string.Empty;

		public string PhoneEntry { get; set; } = string.Empty;

		public ICommand OnLoginButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					OnLoginClicked();
					tcs.SetResult(true);
				});
			}
		}

		public string UserPhone1 { get; set; } = string.Empty;

		private void OnLoginClicked()
		{
			User user = new User
			{
				Name = UsernameEntry,
				PhoneNumber = PhoneEntry
			};

			var isValid = AreCredentialsCorrect(user);
			if (isValid)
			{
				App.IsUserLoggedIn = true;

				//Realm.Write(() =>
				//{
				//	Realm.Add(user, true);
				//});

				CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
			}
			else
			{
				MessageLabel = "Неверно указаны имя или телефон";
			}

		}

		private bool AreCredentialsCorrect(User user)
		{
			if (string.IsNullOrWhiteSpace(user.Name) | string.IsNullOrWhiteSpace(user.PhoneNumber))
			{
				return false;
			}

			return true;
		}
	}
}
