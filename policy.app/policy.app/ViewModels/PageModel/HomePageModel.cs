using System;
using System.Collections.ObjectModel;
using policy.app.Models;
using policy.app.Services;
using Xamarin.Forms;

namespace policy.app.ViewModels.PageModel
{
	public class HomePageModel
	{
		IDataStore<Item> _databaseService;

		//These are automatically filled via Constructor Injection IOC
		public HomePageModel(IDataStore<Item> databaseService)
		{
			_databaseService = databaseService;
		}
	}
}
