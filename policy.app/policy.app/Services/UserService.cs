using System;
using System.Collections.Generic;
using System.Data;
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
	/// Представляет сервис для api пользователя.
	/// </summary>
	public class UserService : IUserService
	{
		private const string EditUri = "http://policy.itmit-studio.ru/api/user/edit";

		private const string UploadAvatarUri = "http://policy.itmit-studio.ru/api/user/changePhoto";

		/// <summary>
		/// Возвращает всех пользователей.
		/// </summary>
		/// <returns>Список пользователей.</returns>
		public IEnumerable<User> GetAllAsync() => throw new NotImplementedException();

		/// <summary>
		/// Возвращает пользователя по <see cref="Guid"/>.
		/// </summary>
		/// <param name="guid">Ид по которому производится поиск.</param>
		/// <returns>Пользователь, с ид указанным в параметре.</returns>
		public User GetUserByGuid(Guid guid) => throw new NotImplementedException();

		/// <summary>
		/// Сохраняет измененные данные пользователя.
		/// </summary>
		/// <param name="user">Пользователь, чьи измененные данные необходимо сохранить.</param>
		public async void Edit(User user)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.Token.TokenType} {user.Token.Token}");

				response = await client.PostAsync(
							   new Uri(EditUri),
							   new FormUrlEncodedContent(new Dictionary<string, string>
							   {
								   {
									   "uid",
									   user.Guid.ToString()
								   },
								   {
									   "name",
									   user.Name
								   },
								   {
									   "city",
									   user.City
								   },
								   {
									   "field_of_activity",
									   user.FieldOfActivity
								   },
								   {
									   "organization",
									   user.Organization
								   },
								   {
									   "position",
									   user.Position
								   }
							   }));
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					return;
				}

				throw new NoNullAllowedException("Нет ответа от сервера.");
			}

			throw new AuthenticationException($"Пользователь с таким токеном, не найден. Токен: {user.Token.Token}");
		}

		/// <summary>
		/// Устанавливает фотографию для аватара, пользователя.
		/// </summary>
		/// <param name="user">Пользователь, которому необходимо установить аватар.</param>
		/// <param name="image">файл, которое нужно установить в качестве аватара пользователя.</param>
		public async void ChangeUserAvatarPhoto(User user, byte[] image)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.Token.TokenType} {user.Token.Token}");

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var byteArrayContent = new ByteArrayContent(image);
				byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
				var content = new MultipartFormDataContent
				{
					{
						byteArrayContent, "\"contents\"", "\"feedback.jpeg\""
					}
				};
				var response = await client.PostAsync($"{UploadAvatarUri}?uid={user.Guid.ToString()}", content);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
			}
		}

		/// <summary>
		/// Отправляет форму обратной связи.
		/// </summary>
		/// <param name="user">Пользователь, от имени которого отправляется форма.</param>
		/// <param name="feedback">Данные из формы обратной связи.</param>
		public void SendFeedBack(User user, Feedback feedback)
		{
			throw new NotImplementedException();
		}
	}
}
