using System;
using System.Collections.Generic;
using AutoMapper;
using policy.app.Models;
using policy.app.RealmObjects;
using Realms;

namespace policy.app.Repositories
{
	public class UserRepository
	{
		private IMapper _mapper;
		private RealmConfiguration _config;

		public UserRepository(RealmConfiguration config)
		{
			_config = config;
			var app = App.Current;
			var mapperConfiguration = app.Configuration;
			_mapper = mapperConfiguration.CreateMapper();
		}

		public void Add(User user)
		{
			UserRealmObject userRealm = _mapper.Map<UserRealmObject>(user);

			using (var realm = Realm.GetInstance(_config))
			{
				using (var transaction = realm.BeginWrite())
				{
					realm.Add(userRealm, true);
					transaction.Commit();
				}
			}
		}

		public IEnumerable<User> All()
		{

			using (var realm = Realm.GetInstance(_config))
			{
				var users = realm.All<UserRealmObject>();
				List<User> userList = new List<User>();
				foreach (var user in users)
				{
					userList.Add(_mapper.Map<User>(user));
				}

				return userList;
			}
		}

		public void Update(User user)
		{
			Remove(user);
			Add(user);
		}

		public void Remove(User user)
		{
			using (var realm = Realm.GetInstance(_config))
			{
				using (var transaction = realm.BeginWrite())
				{
					var userRealm = realm.Find<UserRealmObject>(user.Guid.ToString());
					realm.Remove(userRealm);
					transaction.Commit();
				}
			}
		}
	}
}
