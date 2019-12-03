using Newtonsoft.Json;
using policy.app.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace policy.app.DTO
{
    class UserDto
    {
        #region Properties
        /// <summary>
        /// Возвращает или устанавливает дату рождения пользователя.
        /// </summary>
        public DateTimeOffset Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает город пользователя.
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает почтовый адрес пользователя.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает избранных сусликов.
        /// </summary>
        public IList<Gopher> FavoriteGophers
        {
            get;
        } = new List<Gopher>();

        /// <summary>
        /// Возвращает или устанавливает пол пользователя.
        /// </summary>
        public string Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает образование.
        /// </summary>
        public string Education
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает тип населенного пункта.
        /// </summary>
        public string SettlementType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает регион.
        /// </summary>
        public string Region
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает область деятельности пользователя.
        /// </summary>
        [JsonProperty("field_of_activity")]
        public string FieldOfActivity
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает организацию пользователя.
        /// </summary>
        public string Organization
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает номер телефона пользователя.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber
        {
            get;
            set;
        }
        #endregion

        #region IGopher members
        /// <summary>
        /// Возвращает или устанавливает категорию.
        /// </summary>
        public Category Category
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает ссылку подробной информации.
        /// </summary>
        public string Link
        {
            get;
        }

        /// <summary>
        /// Возвращает или устанавливает количество отрицательных оценок.
        /// </summary>
        public int Dislikes
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает ид пользователя.
        /// </summary>
        [JsonProperty("uid")]
        public Guid Guid
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Возвращает или устанавливает количество положительных оценок.
        /// </summary>
        public int Likes
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает имя пользователя.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает количество нейтральных оценок.
        /// </summary>
        public int Neutrals
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает источник фотографии.
        /// </summary>
        [JsonProperty("photo")]
        public string PhotoSource
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает имя пользователя.
        /// </summary>
        [JsonProperty("place_of_work")]
        public string PlaceOfWork
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает должность пользователя.
        /// </summary>
        public string Position
        {
            get;
            set;
        }
        #endregion
    }
}
