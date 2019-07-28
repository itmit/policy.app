using PropertyChanged;
using Realms;

namespace policy.app.Models
{
	[AddINotifyPropertyChangedInterface]
	public class User : RealmObject
	{
		public string Name
		{
			get;
			set;
		}

		public string PhoneNumber
		{
			get;
			set;
		}
	}
}
