using System.Collections.Generic;

namespace WeatherApp
{
    public class ForecastWeatherInfo
    {
        public class main
        {
            public double temp_min { get; set; }
            public double temp_max { get; set; }

        }
        public class weather 
        {
			public string icon { get; set; }

        }

        public class list 
        {
            public long dt { get; set; }
            public main main { get; set; }
        }

        public class ForecastInfo 
        {
            public List <list> list { get; set; }
            public List <weather> weather { get; set; }
            public main main { get; set; }
        }
    }
}
