using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace WeatherApp
{
	public class WeatherInfo
	{
        public int ID { get; }

        public class coord
		{
			public double lon { get; set; }
			public double lat { get; set; }
		}

		public class ip
		{
			public string query { get; set; }
		}

		public class weather
		{
			public string main { get; set; }
			public string description { get; set; }
			public string icon { get; set; }
		}

		public class main
		{
			public double temp { get; set; }
			public double feels_like { get; set; }
			public double temp_min { get; set; }
			public double temp_max { get; set; }
			public double pressure { get; set; }
			public double humidity { get; set; }

		}

		public class wind
		{
			public double speed { get; set; }
		}

		public class sys
		{
			public string country { get; set; }
			public long sunrise { get; set; }
			public long sunset { get; set; }
		}

		public class list
		{
			public double dt { get; set; }

		}

		public class clouds
		{
			public double all { get; set; }

		}

		public class root
		{
			public coord coord { get; set; }
			public List<weather> weather { get; set; }
			public main main { get; set; }
			public wind wind { get; set; }
			public sys sys { get; set; }
			public string name { get; set; }
			public clouds clouds { get; set; }
			public double visibility { get; set; }
			public ip ip { get; set; }
			public int ID { get; }

		}

		public WeatherInfo(string jsonResponse)
		{
			var jsonData = JObject.Parse(jsonResponse);
			if (jsonData.SelectToken("cod").ToString() == "200")
			{
                ID = int.Parse(jsonData.SelectToken("id").ToString(), CultureInfo.InvariantCulture);
			}
		}
	}
}
