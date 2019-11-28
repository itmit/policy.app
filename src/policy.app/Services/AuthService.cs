using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json;
using policy.app.Models;

namespace policy.app.Services
{
	/// <summary>
	/// Представляет сервис для авторизации.
	/// </summary>
	public class AuthService : IAuthService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Задает адрес авторизации.
		/// </summary>
		private const string AuthUri = "http://policy.itmit-studio.ru/api/login";

		/// <summary>
		/// Адрес для получения пользователя.
		/// </summary>
		private const string DetailsUri = "http://policy.itmit-studio.ru/api/details";

		/// <summary>
		/// Адрес для регистрации пользователя.
		/// </summary>
		private const string RegisterUri = "http://policy.itmit-studio.ru/api/register";

		/// <summary>
		/// Ключ к api для авторизации.
		/// </summary>
		private const string SecretKey = "jENrJI1gVHx16kyly2BMQRONNYctwCDd98FWgn38";

		/// <summary>
		/// Адрес для получения регионов.
		/// </summary>
		private const string GetRegionsUri = "http://policy.itmit-studio.ru/api/getRegions";

		/// <summary>
		/// Адрес для получения файлов в хранилище.
		/// </summary>
		private const string StorageUri = "http://policy.itmit-studio.ru/storage";
		#endregion
		#endregion

		#region .ctor
		#endregion

		#region Public
		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token">Токен для получения пользователя</param>
		/// <returns>Авторизованный пользователь.</returns>
		public async Task<User> GetUserByTokenAsync(UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");

				var response = await client.PostAsync(DetailsUri, null);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
						jsonData.Data.Token = token;
						if (string.IsNullOrEmpty(jsonData.Data.PhotoSource))
						{
							jsonData.Data.PhotoSource = "about:blank";
						}
						else
						{
							jsonData.Data.PhotoSource = jsonData.Data.PhotoSource.Replace("public", "");
							jsonData.Data.PhotoSource = StorageUri + jsonData.Data.PhotoSource;
						}

						return await Task.FromResult(jsonData.Data);
					}
				}

				throw new AuthenticationException($"Пользователь с таким токеном, не найден. Токен: {token.Token}");
			}
		}

		/// <summary>
		/// Отправляет запрос на авторизацию, по api.
		/// </summary>
		/// <param name="login">Логин для авторизации.</param>
		/// <param name="pass">Пароль для авторизации.</param>
		/// <returns>Токен авторизованного пользователя.</returns>
		/// <exception cref="AuthenticationException">Возникает при неудачной авторизации.</exception>
		public async Task<UserToken> LoginAsync(string login, string pass)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"login", login
					},
					{
						"password", pass
					}
				});

				var response = await client.PostAsync(AuthUri, encodedContent);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (string.IsNullOrEmpty(jsonString))
				{
					LastError = "Ошибка сервера.";
					return null;
				}

				if (response.IsSuccessStatusCode)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}

				var jsonError = JsonConvert.DeserializeObject<JsonDataResponse<string>>(jsonString);
				LastError = jsonError.Data;
				return null;
			}
		}

		/// <summary>
		/// Отправляет запрос для регистрации, при помощи rest api.
		/// </summary>
		/// <param name="user">Пользователь, которого необходимо зарегистрировать.</param>
		/// <param name="password">Пароль пользователя.</param>
		/// <param name="confirmPassword">подтверждение пароля.</param>
		/// <returns>Токен нового пользователя.</returns>
		public async Task<UserToken> RegisterAsync(User user, string password, string confirmPassword)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);
				var date = user.Birthday.ToString("yyyy-MM-dd");
				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"email", user.Email
					},
					{
						"phone", user.PhoneNumber
					},
					{
						"birthday", date
					},
					{
						"sex", user.Gender
					},
					{
						"education", user.Education
					},
					{
						"password", password
					},
					{
						"region", user.Region.Name
					},
					{
						"c_password", confirmPassword
					},
					{
						"uid", user.Guid.ToString()
					},
					{
						"city_type", user.SettlementType
					}
				});

				var response = await client.PostAsync(RegisterUri, encodedContent);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (string.IsNullOrEmpty(jsonString))
				{
					LastError = "Ошибка сервера.";
					return null;
				}

				if (response.IsSuccessStatusCode)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}

				var jsonError = JsonConvert.DeserializeObject<JsonDataResponse<string>>(jsonString);
				LastError = jsonError.Data;
				return null;
			}
		}

		/// <summary>
		/// Получает регионы.
		/// </summary>
		/// <returns>Список регионов.</returns>
		public async Task<IEnumerable<Region>> GetRegions()
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.GetAsync(GetRegionsUri);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (string.IsNullOrEmpty(jsonString))
				{
					return new List<Region>();
				}

				if (response.IsSuccessStatusCode)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Region>>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}

				return new List<Region>();
			}
		}

		public string LastError
		{
			get;
			set;
		}
		#endregion
	}
}
