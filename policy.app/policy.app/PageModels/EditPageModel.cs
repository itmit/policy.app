using System.Windows.Input;
using FreshMvvm;
using PropertyChanged;
using Realms;

namespace policy.app.PageModels
{
	[AddINotifyPropertyChangedInterface]
	public class EditPageModel : FreshBasePageModel
	{

		/// <summary>
		/// Возвращает или устанавливает ФИО вводимый пользователем.
		/// </summary>
		public string Name
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает город выводимое пользователю.
		/// </summary>
		public string City
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает сфера деятельности вводимое пользователем.
		/// </summary>
		public string FieldOfActivity
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает организацию.
		/// </summary>
		public string Organization
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает должность.
		/// </summary>
		public string Position
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает команду для кнопки регистрации.
		/// </summary>
		public ICommand OnSaveButtonClicked
		{
			get
			{
				return new FreshAwaitCommand((param, tcs) =>
				{
					SaveChangesAsync();
				});
			}
		}

		/// <summary>
		/// Сохраняет введенные пользователем данные.
		/// </summary>
		private void SaveChangesAsync()
		{

		}

		private Realm Realm => Realm.GetInstance();
	}
}
