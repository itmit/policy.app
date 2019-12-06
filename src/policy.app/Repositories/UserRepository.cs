using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using policy.app.DTO;
using policy.app.Models;
using policy.app.RealmObjects;
using Realms;

namespace policy.app.Repositories
{
	public class UserRepository
	{
		#region Data
		#region Fields
		private readonly RealmConfiguration _config;
		private readonly IMapper _mapper;
		#endregion
		#endregion

		#region .ctor
		public UserRepository(RealmConfiguration config)
		{
			_config = config;
			var app = App.Current;
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddCollectionMappers();

				cfg.CreateMap<User, UserRealmObject>()
				   .ForMember(q => q.FavoriteGophers, opt => opt.MapFrom(q => q.FavoriteGophers))
				   .ForPath(m => m.Region, o => o.MapFrom(q => q.Region.Name));

				cfg.CreateMap<Gopher, GopherRealmObject>();
				cfg.CreateMap<UserToken, TokenRealmObject>();
				cfg.CreateMap<Category, CategoryRealmObject>();

				cfg.CreateMap<UserRealmObject, User>()
				   .ForPath(m => m.Region.Name, o => o.MapFrom(q => q.Region))
				   .ForMember(q => q.FavoriteGophers, opt => opt.MapFrom(q => q.FavoriteGophers));

				cfg.CreateMap<GopherRealmObject, Gopher>();
				cfg.CreateMap<TokenRealmObject, UserToken>();
				cfg.CreateMap<CategoryRealmObject, Category>();
			});
			_mapper = mapperConfiguration.CreateMapper();
		}
		#endregion

		#region Public
		public void Add(User user)
		{
			var userRealm = _mapper.Map<UserRealmObject>(user);

			using (var realm = Realm.GetInstance(_config))
			{
				using (var transaction = realm.BeginWrite())
				{
					realm.Add((RealmObject) userRealm, true);
					transaction.Commit();
				}
			}
		}

		public IEnumerable<User> All()
		{
			using (var realm = Realm.GetInstance(_config))
			{
				var users = realm.All<UserRealmObject>();
				var userList = new List<User>();
				foreach (var user in users)
				{
					userList.Add(_mapper.Map<User>(user));
				}

				return userList;
			}
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

		public void Update(User user)
		{
			Remove(user);
			Add(user);
		}
		#endregion
	}
}
