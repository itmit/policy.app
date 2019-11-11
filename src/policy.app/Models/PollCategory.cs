using System;
using Newtonsoft.Json;

namespace policy.app.Models
{
	public class PollCategory
	{
		[JsonProperty("uuid")]
		public Guid Guid
		{
			get;
			set;
		}

		public string ImageSource
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
	}
}
