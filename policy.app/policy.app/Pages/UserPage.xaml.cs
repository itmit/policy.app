﻿using System;
using FFImageLoading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FFImageLoading.Views;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserPage : ContentPage
	{
		#region .ctor
		public UserPage()
		{
			InitializeComponent();
			
		}
		#endregion

		#region Private
		private void Button_Clicked(object sender, EventArgs e)
		{
			Browser.OpenAsync("https://ru.wikipedia.org/wiki/", BrowserLaunchMode.SystemPreferred);
		}
		#endregion
	}
}
