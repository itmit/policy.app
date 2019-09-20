using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using policy.app.Models;
using static System.String;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для работы с сусликами и их категориями.
	/// </summary>
	public class GopherService : IGopherService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Адрес для добавления в избранное.
		/// </summary>
		private const string AddToFavoritesUri = "http://policy.itmit-studio.ru/api/suslik/addToFav";

		private const string FavoritesGophersUri = "http://policy.itmit-studio.ru/api/suslik/getFavsList";

		/// <summary>
		/// Адрес для получения категорий.
		/// </summary>
		private const string CategoriesUri = "http://policy.itmit-studio.ru/api/suslik/getCategoryList";

		/// <summary>
		/// Адрес для установки оценок.
		/// </summary>
		private const string RateUri = "http://policy.itmit-studio.ru/api/suslik/rateSuslik";

		/// <summary>
		/// Адрес для получения сусликов по категории.
		/// </summary>
		private const string GophersUri = "http://policy.itmit-studio.ru/api/suslik/getSusliksByCategory";

		/// <summary>
		/// Адрес для получения суслика по ид.
		/// </summary>
		private const string GopherUri = "http://policy.itmit-studio.ru/api/suslik/getSuslikByID";
		#endregion

		#region Fields
		/// <summary>
		/// Токен для авторизации.
		/// </summary>
		private readonly UserToken _token;
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="GopherService" />.
		/// </summary>
		/// <param name="token">Токен для авторизации.</param>
		/// <exception cref="ArgumentNullException">Возникает если переданный токен равен <c>null</c>.</exception>
		/// <exception cref="ArgumentException">Возникает если токен пуст или равен <c>null</c></exception>
		public GopherService(UserToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (IsNullOrEmpty(token.Token))
			{
				throw new ArgumentException("Токен не должен быть пуст.");
			}

			_token = token;
		}
		#endregion

		#region IGopherService members
		/// <summary>
		/// Возвращает все категории сусликов.
		/// </summary>
		/// <returns>Список категорий.</returns>
		public async Task<IEnumerable<Category>> GetCategories()
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(CategoriesUri, null);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Category>>>(jsonString);

				if (jsonData.Data != null)
				{
					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new List<Category>());
			}
		}

		/// <summary>
		/// Возвращает суслики по ид.
		/// </summary>
		/// <param name="guid">ид суслика.</param>
		/// <returns>Суслик.</returns>
		public async Task<Gopher> GetGopher(Guid guid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(GopherUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "suslik_uuid", guid.ToString()
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<Gopher>>(jsonString);
				if (jsonData.Data != null)
				{

					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new Gopher());
			}
		}

		public async Task<bool> Rate(IGopher gopher, RateType rateType)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(RateUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "suslik_uuid", gopher.Guid
														  },
														  {
															  "type", rateType.ToString()
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Добавить суслика в избранное.
		/// </summary>
		/// <param name="addedGopher">Добавляемый пользователь.</param>
		/// <param name="addingGopher">Добавляющий пользователь.</param>
		/// <returns>Был ли добавлен пользователь в избранное.</returns>
		public async Task<bool> AddToFavorites(IGopher addedGopher, IGopher addingGopher)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(AddToFavoritesUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "suslik_uuid", addedGopher.Guid
														  },
														  {
															  "user_uuid", addingGopher.Guid
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Возвращает сусликов добавленных в избранное.
		/// </summary>
		/// <param name="gopher">Пользователь добавивший сусликов в избранное.</param>
		/// <returns>Список сусликов.</returns>
		public async Task<IEnumerable<IGopher>> GetFavorites(IGopher gopher)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(FavoritesGophersUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "user_uuid", gopher.Guid
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Gopher>>>(jsonString);
				foreach (var gopher1 in jsonData.Data)
				{
					gopher1.Name = "Смелов Сергей Константинович";
					gopher1.Guid = "31dfde9c-af9e-468b-add6-dffff78419e8";
				}
				return await Task.FromResult(jsonData.Data);
			}
		}

		/// <summary>
		/// Возвращает всех сусликов категории.
		/// </summary>
		/// <param name="category">Категория, отбираемых сусликов.</param>
		/// <returns>Список сусликов по категории.</returns>
		public async Task<IEnumerable<Gopher>> GetGophers(Category category)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(GophersUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "category_uuid", category.Uuid
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Gopher>>>(jsonString);
				foreach (var gopher in jsonData.Data)
				{
					gopher.Category = category;
				}
				return await Task.FromResult(jsonData.Data);
			}
		}
		#endregion
	}
}
