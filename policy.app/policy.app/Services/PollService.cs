using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для работы с опросами по api.
	/// </summary>
	public class PollService: IPollService
	{
		/// <summary>
		/// Адрес для получения опросов.
		/// </summary>
		private const string GetPullUri = "http://policy.itmit-studio.ru/api/poll/getPollList";

		private const string GetPullQuestionsUri = "http://policy.itmit-studio.ru/api/poll/getPollQuestionList";

		/// <summary>
		/// Токен пользователя для доступа к api.
		/// </summary>
		private UserToken _token;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="PollService"/>.
		/// </summary>
		/// <param name="token">Токен пользователя, для доступа к api.</param>
		public PollService(UserToken token) => _token = token;

		/// <summary>
		/// Возвращает список опросов.
		/// </summary>
		/// <returns>Список опросов.</returns>
		public async Task<IEnumerable<Poll>> GetPolls()
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(GetPullUri, null);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Poll>>>(jsonString);

				if (jsonData.Data != null)
				{
					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new List<Poll>());
			}
		}

		/// <summary>
		/// Возвращает список вопросов опроса.
		/// </summary>
		/// <param name="guid">Ид опроса.</param>
		/// <returns>Список вопросов.</returns>
		public async Task<IEnumerable<Question>> GetQuestions(Guid guid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(GetPullQuestionsUri, null);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Question>>>(jsonString);

				if (jsonData.Data != null)
				{
					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new List<Question>());
			}
		}
	}
}
