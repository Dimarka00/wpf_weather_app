using System;
using System.Data.SqlClient;

namespace WeatherApp
{
    public class WeatherInfoHandler
    {
        public void SaveWeatherDataOnStart(WeatherInfo.root weatherData)
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string insertQuery = @"INSERT INTO WeatherDataOnStart (Temperature, FeelsLike, TempMin, TempMax, Pressure, Humidity,
                                WindSpeed, Country, SunriseTime, SunsetTime, CityName, Cloudiness, Visibility, DateAdded)
                                VALUES (@Temperature, @FeelsLike, @TempMin, @TempMax, @Pressure, @Humidity,
                                @WindSpeed, @Country, @SunriseTime, @SunsetTime, @CityName, @Cloudiness, @Visibility, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Temperature", weatherData.main.temp);
                        cmd.Parameters.AddWithValue("@FeelsLike", weatherData.main.feels_like);
                        cmd.Parameters.AddWithValue("@TempMin", weatherData.main.temp_min);
                        cmd.Parameters.AddWithValue("@TempMax", weatherData.main.temp_max);
                        cmd.Parameters.AddWithValue("@Pressure", weatherData.main.pressure);
                        cmd.Parameters.AddWithValue("@Humidity", weatherData.main.humidity);
                        cmd.Parameters.AddWithValue("@WindSpeed", weatherData.wind.speed);
                        cmd.Parameters.AddWithValue("@Country", weatherData.sys.country);
                        cmd.Parameters.AddWithValue("@SunriseTime", weatherData.sys.sunrise);
                        cmd.Parameters.AddWithValue("@SunsetTime", weatherData.sys.sunset);
                        cmd.Parameters.AddWithValue("@CityName", weatherData.name);
                        cmd.Parameters.AddWithValue("@Cloudiness", weatherData.clouds.all);
                        cmd.Parameters.AddWithValue("@Visibility", weatherData.visibility);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при записи данных: " + ex.Message);
                }
            }
        }

        public void SaveWeatherDataOnSearch(WeatherInfo.root weatherData)
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string insertQuery = @"INSERT INTO WeatherDataOnSearch (Temperature, FeelsLike, TempMin, TempMax, Pressure, Humidity,
                                WindSpeed, Country, SunriseTime, SunsetTime, CityName, Cloudiness, Visibility, DateAdded)
                                VALUES (@Temperature, @FeelsLike, @TempMin, @TempMax, @Pressure, @Humidity,
                                @WindSpeed, @Country, @SunriseTime, @SunsetTime, @CityName, @Cloudiness, @Visibility, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Temperature", weatherData.main.temp);
                        cmd.Parameters.AddWithValue("@FeelsLike", weatherData.main.feels_like);
                        cmd.Parameters.AddWithValue("@TempMin", weatherData.main.temp_min);
                        cmd.Parameters.AddWithValue("@TempMax", weatherData.main.temp_max);
                        cmd.Parameters.AddWithValue("@Pressure", weatherData.main.pressure);
                        cmd.Parameters.AddWithValue("@Humidity", weatherData.main.humidity);
                        cmd.Parameters.AddWithValue("@WindSpeed", weatherData.wind.speed);
                        cmd.Parameters.AddWithValue("@Country", weatherData.sys.country);
                        cmd.Parameters.AddWithValue("@SunriseTime", weatherData.sys.sunrise);
                        cmd.Parameters.AddWithValue("@SunsetTime", weatherData.sys.sunset);
                        cmd.Parameters.AddWithValue("@CityName", weatherData.name);
                        cmd.Parameters.AddWithValue("@Cloudiness", weatherData.clouds.all);
                        cmd.Parameters.AddWithValue("@Visibility", weatherData.visibility);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при записи данных: " + ex.Message);
                }
            }
        }

    }
}
