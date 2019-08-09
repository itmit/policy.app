using System.Net.Mime;
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
		/// <summary>
		/// Возвращает команду для авторизации.
		/// </summary>
		public ICommand OpenAppCommand
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					Application.Current.MainPage = App.GeMainTabbedPage();
				});
			}
		}
	}
}
