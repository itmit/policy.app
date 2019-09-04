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
		/// Адрес для получения категорий.
		/// </summary>
		private const string CategoriesUri = "http://policy.itmit-studio.ru/api/suslik/getCategoryList";

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

				var response = await client.PostAsync(CategoriesUri, new FormUrlEncodedContent(new Dictionary<string, string>()));
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
															  "suslik_id", guid.ToString()
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<Gopher>>(jsonString);
				if (jsonData.Data == null)
				{

					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new Gopher());
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

				return await Task.FromResult(jsonData.Data);
			}
		}
		#endregion
	}
}
