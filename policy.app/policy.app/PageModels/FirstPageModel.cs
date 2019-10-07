using System.Windows.Input;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace policy.app.PageModels
{
	/// <summary>
	/// Представляет модель представления для первой, приветственной страницы.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public class FirstPageModel : FreshBasePageModel
	{
		#region Data
		#region Fields
		private App _app;
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает команду для авторизации.
		/// </summary>
		public ICommand OpenAppCommand
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					_app = Application.Current as App;
					if (_app == null)
					{
						return;
					}

					Application.Current.MainPage = _app.InitMainTabbedPage();
				});
			}
		}
		#endregion
	}
}
