using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для работы с опросами по api.
	/// </summary>
	public class PollService : IPollService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Адрес для получения вопросов.
		/// </summary>
		private const string GetPullQuestionsUri = "http://policy.itmit-studio.ru/api/poll/getPollQuestionList";

		/// <summary>
		/// Адрес для получения опросов.
		/// </summary>
		private const string GetPullUri = "http://policy.itmit-studio.ru/api/poll/getPollList";

		/// <summary>
		/// Адрес для прохождения опроса.
		/// </summary>
		private const string PassPullUri = "http://policy.itmit-studio.ru/api/poll/passPoll";
		#endregion

		#region Fields
		/// <summary>
		/// Клиент для отправки запросов.
		/// </summary>
		private readonly HttpClient _httpClient;

		/// <summary>
		/// Токен пользователя для доступа к api.
		/// </summary>
		private readonly UserToken _token;
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="PollService" />.
		/// </summary>
		/// <param name="token">Токен пользователя, для доступа к api.</param>
		/// <param name="httpClient"></param>
		public PollService(UserToken token, HttpClient httpClient)
		{
			_token = token;
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
		#endregion

		#region IPollService members
		/// <summary>
		/// Возвращает список опросов.
		/// </summary>
		/// <returns>Список опросов.</returns>
		public async Task<IEnumerable<Poll>> GetPolls()
		{
			using (_httpClient)
			{
				_httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");

				var response = await _httpClient.PostAsync(GetPullUri, null);
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
			using (_httpClient)
			{
				_httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");

				var response = await _httpClient.PostAsync(GetPullQuestionsUri,
														   new FormUrlEncodedContent(new Dictionary<string, string>
														   {
															   {
																   "poll_uuid", guid.ToString()
															   }
														   }));
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

		/// <summary>
		/// Отправляет запрос на прохождение опроса.
		/// </summary>
		/// <param name="poll">Проходимый опрос.</param>
		/// <param name="user">Пользователь, который проходит опрос.</param>
		/// <returns>Возвращает был ли удачно пройден опрос.</returns>
		public async Task<bool> PassPull(Poll poll, User user)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var fields = new Dictionary<string, string>();
				foreach (var question in poll.Questions)
				{
					foreach (var answer in question.Answers)
					{
						if (!answer.IsSelected)
						{
							continue;
						}

						fields.Add($"user_answer[{question.Guid}][{answer.Guid}]", answer.OtherText);
					}
				}

				var response = await client.PostAsync(PassPullUri,
														   new FormUrlEncodedContent(fields));
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}
		#endregion
	}
}
