using Xamarin.Forms;

namespace policy.app.Controls
{
	/// <summary>
	/// Представляет слайдер, для рейтинга.
	/// </summary>
	public class SliderRating : Slider
	{
		#region Data
		#region Static
		/// <summary>
		/// Свойство для привязки цвет при максимальном значении.
		/// </summary>
		public static readonly BindableProperty MaxColorProperty = BindableProperty.Create(nameof(MaxColor), typeof(Color), typeof(SliderRating), Color.Default);

		/// <summary>
		/// Свойство для привязки при минимальном значении.
		/// </summary>
		public static readonly BindableProperty MinColorProperty = BindableProperty.Create(nameof(MinColor), typeof(Color), typeof(SliderRating), Color.Default);
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает цвет при максимальном значении.
		/// </summary>
		public Color MaxColor
		{
			get => (Color) GetValue(MaxColorProperty);
			set => SetValue(MaxColorProperty, value);
		}

		/// <summary>
		/// Возвращает или устанавливает цвет при минимальном значении.
		/// </summary>
		public Color MinColor
		{
			get => (Color) GetValue(MinColorProperty);
			set => SetValue(MinColorProperty, value);
		}
		#endregion
	}
}
