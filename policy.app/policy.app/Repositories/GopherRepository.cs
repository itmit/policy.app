using System.Collections.Generic;
using policy.app.Models;
using policy.app.RealmObjects;
using Realms;

namespace policy.app.Repositories
{
	public class GopherRepository
	{
		private Realm _realm;

		public GopherRepository(RealmConfiguration config)
		{
			_realm = Realm.GetInstance(config);
		}

		public void Add(Gopher gopher)
		{
			var app = App.Current;
			var config = app.Configuration;

			var mapper = config.CreateMapper();

			GopherRealmObject gopherRealm = mapper.Map<Gopher, GopherRealmObject>(gopher);

			using (_realm)
			{
				using (var transaction = _realm.BeginWrite())
				{
					_realm.Add(gopherRealm, true);
					transaction.Commit();
				}
			}
		}

		public IEnumerable<Gopher> All()
		{
			var app = App.Current;
			var config = app.Configuration;

			var mapper = config.CreateMapper();

			using (_realm)
			{
				var gophers = _realm.All<GopherRealmObject>();
				List<Gopher> gopherList = new List<Gopher>();
				foreach (var gopher in gophers)
				{
					gopherList.Add(mapper.Map<Gopher>(gopher));
				}

				return gopherList;
			}
		}
	}
}