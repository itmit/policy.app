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

		private const string GetPollCategoriesUri = "http://policy.itmit-studio.ru/api/poll/getPollCategoryList";
		private const string GetPollSubCategoriesUri = "http://policy.itmit-studio.ru/api/poll/getSubCategoryList";

		/// <summary>
		/// Адрес для прохождения опроса.
		/// </summary>
		private const string PassPullUri = "http://policy.itmit-studio.ru/api/poll/passPoll";
		#endregion

		#region Fields
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
		public PollService(UserToken token)
		{
			_token = token;
		}
		#endregion

		#region IPollService members

		public async Task<IEnumerable<PollCategory>> GetPollCategories(Guid? uuid = null)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				HttpResponseMessage response;
				if (uuid == null)
				{
					response = await client.PostAsync(GetPollCategoriesUri, null);
				}
				else
				{
					response = await client.PostAsync(GetPollSubCategoriesUri, new FormUrlEncodedContent(new Dictionary<string, string>
					{
						{"uuid", uuid.Value.ToString() }
					}));
				}

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<PollCategory>>>(jsonString);

				if (jsonData.Data != null)
				{
					return await Task.FromResult(jsonData.Data);
				}

				return await Task.FromResult(new List<PollCategory>());
			}
		}

		/// <summary>
		/// Возвращает список опросов.
		/// </summary>
		/// <returns>Список опросов.</returns>
		public async Task<IEnumerable<Poll>> GetPolls(Guid pollCategoryGuid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				FormUrlEncodedContent content;
				if (pollCategoryGuid.Equals(Guid.Empty))
				{
					content = null;
				}
				else
				{
					content = new FormUrlEncodedContent(
						new Dictionary<string, string>
						{
							{
								"category_uuid", pollCategoryGuid.ToString()
							}
						});
				}
				var response = await client.PostAsync(GetPullUri, content);
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
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");

				var response = await client.PostAsync(GetPullQuestionsUri,
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
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
				fields.Add("uuid", poll.Guid.ToString());
				var response = await client.PostAsync(PassPullUri,
														   new FormUrlEncodedContent(fields));
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}

				var data = JsonConvert.DeserializeObject<JsonDataResponse<string>>(jsonString);
				Error = data.Data;
				ErrorMessage = data.Message;
				return false;
			}
		}

		public string ErrorMessage
		{
			get;
			set;
		}

		public string Error
		{
			get;
			set;
		}
		#endregion
	}
}
