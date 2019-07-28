﻿using System;
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
	public class AuthService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Задает адрес авторизации.
		/// </summary>
		private const string AuthUri = "http://policy.itmit-studio.ru/api/login";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string DetailsUri = "http://policy.itmit-studio.ru/api/details";

		/// <summary>
		/// Задает адрес для регистрации пользователя.
		/// </summary>
		private const string RegisterUri = "http://policy.itmit-studio.ru/api/register";

		/// <summary>
		/// Задает ключ к api для авторизации.
		/// </summary>
		private const string SecretKey = "jENrJI1gVHx16kyly2BMQRONNYctwCDd98FWgn38";
		#endregion
		#endregion

		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<User> GetUserByTokenAsync(UserToken token)
		{

			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");

				response = await client.PostAsync(new Uri(DetailsUri), null);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<User> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
					jsonData.Data.Token = token;
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthenticationException($"Пользователь с таким токеном, не найден. Токен: {token.Token}");

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
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string> {
					{
						"login",
						login
					},
					{
						"password",
						pass
					}
				});

				response = await client.PostAsync(new Uri(AuthUri), encodedContent);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<UserToken> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthenticationException($"Возникла ошибка при авторизации. Логин: {login}");
		}

		public async Task<UserToken> RegisterAsync(User user, string password, string confirmPassword)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string> {
					{
						"email",
						user.Email
					},
					{
						"phone",
						user.PhoneNumber
					},
					{
						"password",
						password
					},
					{
						"c_password",
						confirmPassword
					},
					{
						"birthday",
						user.Birthday.ToString()
					},
					{
						"uid",
						user.StoredUserGuidString
					}
				});

				response = await client.PostAsync(new Uri(AuthUri), encodedContent);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<UserToken> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthenticationException($"Возникла ошибка при авторизации. Логин: ");
		}
	}
}
