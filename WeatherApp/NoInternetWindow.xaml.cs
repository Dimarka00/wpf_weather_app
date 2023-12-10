using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;
using WeatherApp.Utils;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for NoInternetWindow.xaml
    /// </summary>
    public partial class NoInternetWindow : Window
    {
        private MainWindow _mainWindow;

        private WeatherDataManager _dataManager;

        public NoInternetWindow()
        {
            InitializeComponent();
        }
        private void RetryButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dataManager.CheckInternetConnection())
                {
                    string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

                    using (WebClient web = new WebClient())
                    {
                        string apiKey = _mainWindow.GetApiKeyFromDatabase(connectionString); // Получаем API_KEY из базы данных

                        string ipInfoUrl = "http://ipinfo.io/ip";
                        string userIpAddress = web.DownloadString(ipInfoUrl).Trim();

                        string locationInfoUrl = $"http://ip-api.com/json/{userIpAddress}";
                        var locationJson = web.DownloadString(locationInfoUrl);
                        LocationInfo locationData = JsonConvert.DeserializeObject<LocationInfo>(locationJson);

                        string weatherUrl = string.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&lang=ru", locationData.Lat, locationData.Lon, apiKey);
                        var json = web.DownloadString(weatherUrl);

                        WeatherInfo.root weatherData = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                        _mainWindow.UpdateWeatherUI(weatherData);

                        WeatherInfoHandler weatherInfoHandler = new WeatherInfoHandler();
                        weatherInfoHandler.SaveWeatherDataOnStart(weatherData);
                    }
                }
                else
                {
                    _dataManager.ShowNoInternetMessage();
                }
            }
            catch (Exception ex)
            {
                _dataManager.ShowNoInternetMessage();
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}