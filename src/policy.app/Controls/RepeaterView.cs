using System.Collections;
using Xamarin.Forms;

namespace policy.app.Controls
{
	/// <summary>
	/// Представляет <see cref="View"/>, с возможность установить источник элементов.
	/// </summary>
	public class RepeaterView : StackLayout
	{
		#region Data
		#region Static
		/// <summary>
		/// Свойство шаблона элемента.
		/// </summary>
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
			nameof(ItemTemplate),
			typeof(DataTemplate),
			typeof(RepeaterView),
			default(DataTemplate));

		/// <summary>
		/// Свойство источника элементов.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(ICollection),
			typeof(RepeaterView),
			null,
			BindingMode.OneWay,
			propertyChanged: ItemsChanged);
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="RepeaterView"/>.
		/// </summary>
		public RepeaterView() => Spacing = 0;
		#endregion

		#region Properties

		/// <summary>
		/// Возвращает или устанавливает источник элементов.
		/// </summary>
		public ICollection ItemsSource
		{
			get => (ICollection) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// Возвращает или устанавливает шаблон вывода для элемнета.
		/// </summary>
		public DataTemplate ItemTemplate
		{
			get => (DataTemplate) GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}
		#endregion

		#region Overridable

		/// <summary>
		/// Возвращает <see cref="View"/> с контекстом.
		/// </summary>
		/// <param name="item">Контекст.</param>
		/// <returns><see cref="View"/> с контекстом.</returns>
		protected virtual View ViewFor(object item)
		{
			View view = null;

			if (ItemTemplate != null)
			{
				var content = ItemTemplate.CreateContent();

				view = content is View view1 ? view1 : ((ViewCell) content).View;

				view.BindingContext = item;
			}

			return view;
		}
		#endregion

		#region Private
		/// <summary>
		/// Вызывается при изменении источника элементов.
		/// </summary>
		/// <param name="bindable">Привязанный объект.</param>
		/// <param name="oldValue">Старое значение.</param>
		/// <param name="newValue">Новое значение.</param>
		private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = bindable as RepeaterView;

			if (control == null)
			{
				return;
			}

			control.Children.Clear();

			var items = (ICollection) newValue;

			if (items == null)
			{
				return;
			}

			foreach (var item in items)
			{
				control.Children.Add(control.ViewFor(item));
			}
		}
		#endregion
	}
}
