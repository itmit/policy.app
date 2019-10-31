﻿using System;
using Newtonsoft.Json;
using PropertyChanged;

namespace policy.app.Models
{
	[AddINotifyPropertyChangedInterface]
	public class Gopher : IGopher
	{
		#region IGopher members
		/// <summary>
		/// Возвращает или устанавливает категорию.
		/// </summary>
		public Category Category
		{
			get;
			set;
		}

		public string Link
		{
			get;
			set;
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
		[JsonProperty("uuid")]
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
