using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Windows.Threading;
using WeatherApp.UserControls;
using WeatherApp.Utils;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private WeatherDataManager _dataManager;

        private WeatherInfo currentResult;

        private bool internetAvailable = false; // Переменная для отслеживания доступности интернета

        private MainWindow _mainWindow;

        private BitmapImage weatherIcon;

        string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

        private DispatcherTimer timer;

        //public string API_KEY = "e03d22b39b46c902968bd7a5946f5caf";

        double lon;
        double lat;

        public MainWindow()
        {
            _dataManager = new WeatherDataManager();

            InitializeComponent();
            InitializeTimer();
            getWeatherOnStart();
            getForecastWeatherOnStart();

            backgroundVideo.Loaded += BackgroundVideo_Loaded;
            backgroundVideo.MediaEnded += BackgroundVideo_MediaEnded;

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            string formattedDateTime = now.ToString("HH:mm, dddd");

            dateNow.Text = formattedDateTime;
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    getWeather();
                    getForecastWeather();
                }
                catch (Exception ex)
                {
                    if (ex is WebException webEx && webEx.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Город не найден
                        MessageBox.Show("Город не найден.");
                    }
                    else
                    {
                        // Ошибка при получении данных о погоде
                        Console.WriteLine("Ошибка: " + ex.Message);
                        MessageBox.Show("Произошла ошибка при получении данных о погоде");
                    }
                }
            }
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
                textSearch.Visibility = Visibility.Collapsed;
            else
                textSearch.Visibility = Visibility.Visible;
        }

        void getWeather()
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            if (_dataManager.CheckInternetConnection())
            {
                using (WebClient web = new WebClient())
                {
                    string apiKey = GetApiKeyFromDatabase(connectionString); // Получаем API_KEY из базы данных

                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=ru", txtSearch.Text, apiKey);
                    var json = web.DownloadString(url);

                    WeatherInfo.root weatherData = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    UpdateWeatherUI(weatherData);

                    WeatherInfoHandler weatherInfoHandler = new WeatherInfoHandler();
                    weatherInfoHandler.SaveWeatherDataOnSearch(weatherData);
                }
            }
        }

        public void getWeatherOnStart()
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            if (_dataManager.CheckInternetConnection()) 
            {
                using (WebClient web = new WebClient())
                {
                    string apiKey = GetApiKeyFromDatabase(connectionString);

                    string ipInfoUrl = "http://ipinfo.io/ip";
                    string userIpAddress = web.DownloadString(ipInfoUrl).Trim();

                    string locationInfoUrl = $"http://ip-api.com/json/{userIpAddress}";
                    var locationJson = web.DownloadString(locationInfoUrl);
                    LocationInfo locationData = JsonConvert.DeserializeObject<LocationInfo>(locationJson);

                    string weatherUrl = string.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&lang=ru", locationData.Lat, locationData.Lon, apiKey);
                    var json = web.DownloadString(weatherUrl);

                    WeatherInfo.root weatherData = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    UpdateWeatherUI(weatherData);

                    WeatherInfoHandler weatherInfoHandler = new WeatherInfoHandler();
                    weatherInfoHandler.SaveWeatherDataOnStart(weatherData);
                }
            }
        }
        DateTime convertDataTime(long millisec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisec);
            return day;
        }
        void getForecastWeather()
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            if (_dataManager.CheckInternetConnection()) 
            {
                using (WebClient web = new WebClient())
                {
                    string apiKey = GetApiKeyFromDatabase(connectionString); // Получаем API_KEY из базы данных

                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units=metric&lang=ru&cnt=30", txtSearch.Text, apiKey);
                    var json = web.DownloadString(url);
                    ForecastWeatherInfo.ForecastInfo ForecastInfo = JsonConvert.DeserializeObject<ForecastWeatherInfo.ForecastInfo>(json);
                    UpdateForecastUI(ForecastInfo);
                }
            }
        }
        public void getForecastWeatherOnStart()
        {
            string connectionString = "Data Source=DESKTOP-SJ4I1LF;Initial Catalog=WeatherDatabase;Integrated Security=True;";

            if (_dataManager.CheckInternetConnection()) 
            {
                using (WebClient web = new WebClient())
                {
                    string apiKey = GetApiKeyFromDatabase(connectionString); // Получаем API_KEY из базы данных

                    string ipInfoUrl = "http://ipinfo.io/ip";
                    string userIpAddress = web.DownloadString(ipInfoUrl).Trim();

                    string locationInfoUrl = $"http://ip-api.com/json/{userIpAddress}";
                    var locationJson = web.DownloadString(locationInfoUrl);
                    LocationInfo locationData = JsonConvert.DeserializeObject<LocationInfo>(locationJson);

                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid={2}&units=metric&lang=ru&cnt=30", locationData.Lat, locationData.Lon, apiKey);
                    var json = web.DownloadString(url);
                    ForecastWeatherInfo.ForecastInfo ForecastInfo = JsonConvert.DeserializeObject<ForecastWeatherInfo.ForecastInfo>(json);

                    UpdateForecastUI(ForecastInfo);
                }
            }
        }
        public void UpdateWeatherUI(WeatherInfo.root weatherData)
        {
            string iconUrl = "http://openweathermap.org/img/w/" + weatherData.weather[0].icon + ".png";
            BitmapImage weatherIcon = new BitmapImage();
            weatherIcon.BeginInit();
            weatherIcon.UriSource = new Uri(iconUrl);
            weatherIcon.EndInit();
            imgWeatherIcon.Source = weatherIcon;
            imgWeatherIconSmall.Source = weatherIcon;


            int roundedTemperature = (int)Math.Round(weatherData.main.temp);
            mainTemp.Text = $"{roundedTemperature}°C";

            weatherConditions.Text = TranslateWeatherDescription(weatherData.weather[0].main);

            double pressure = weatherData.main.pressure / 1.333;
            int roundedPressure = (int)Math.Round(pressure);
            pressureNow.Text = $"{roundedPressure} мм рт. ст.";

            cityBlock.Text = weatherData.name.ToString();

            labWindSpeed.Text = weatherData.wind.speed.ToString();

            labSunrise.Text = convertDataTime(weatherData.sys.sunrise).ToShortTimeString();
            labSunset.Text = convertDataTime(weatherData.sys.sunset).ToShortTimeString();

            labHumidity.Text = weatherData.main.humidity.ToString() + "%";

            int roundedFeelsLike = (int)Math.Round(weatherData.main.feels_like);
            labFeelsLike.Text = $"{roundedFeelsLike}°C";


            labClouds.Text = $"{weatherData.clouds.all.ToString()}%";

            double visibilityKilometres = weatherData.visibility / 1000;
            labVisibility.Text = $"{visibilityKilometres} км";

            lon = weatherData.coord.lon;
            lat = weatherData.coord.lat;
        }
        private void UpdateForecastUI(ForecastWeatherInfo.ForecastInfo forecastInfo)
        {
            FLP.Children.Clear();

            Dictionary<DateTime, (double MaxTemp, double MinTemp)> dailyTemperatureData = new Dictionary<DateTime, (double, double)>();

            foreach (var item in forecastInfo.list)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(item.dt).Date;

                if (!dailyTemperatureData.ContainsKey(dateTime))
                {
                    dailyTemperatureData[dateTime] = (item.main.temp_max, item.main.temp_min);
                }
                else
                {
                    var currentData = dailyTemperatureData[dateTime];
                    dailyTemperatureData[dateTime] = (
                        Math.Max(currentData.MaxTemp, item.main.temp_max),
                        Math.Min(currentData.MinTemp, item.main.temp_min)
                    );
                }
            }

            // Карта перевода дней недели на русский язык
            string[] russianDayNames = new string[]
            {
        "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"
            };

            foreach (var entry in dailyTemperatureData)
            {
                double maxTemp = entry.Value.MaxTemp;
                double minTemp = entry.Value.MinTemp;
                int roundedMaxTemperature = (int)Math.Round(maxTemp);
                int roundedMinTemperature = (int)Math.Round(minTemp);

                // Получаем название дня недели на русском языке
                string dayOfWeek = russianDayNames[(int)entry.Key.DayOfWeek];

                // Создаем экземпляр CardDay
                CardDay CD = new CardDay();

                // Назначаем источник изображения до добавления CardDay в FLP
                CD.lbIcon.Source = weatherIcon;

                CD.lbMaxTemp.Text = $"{roundedMaxTemperature}°C".ToString();
                CD.lbMinTemp.Text = $"{roundedMinTemperature}°C".ToString();
                CD.lbDay.Text = dayOfWeek; // Устанавливаем название дня недели на русском языке

                // Добавляем CardDay в FLP
                FLP.Children.Add(CD);
            }

        }
        public string GetApiKeyFromDatabase(string connectionString)
        {
            string apiKey = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 KeyValue FROM ApiKeys";

                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    apiKey = reader.GetString(0);
                }

                reader.Close();
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = "e03d22b39b46c902968bd7a5946f5caf";
            }

            return apiKey;
        }
        public string TranslateWeatherDescription(string weatherMain)
        {
            switch (weatherMain)
            {
                case "Thunderstorm":
                    return "Гроза";
                case "Drizzle":
                    return "Морось";
                case "Rain":
                    return "Дождь";
                case "Snow":
                    return "Снег";
                case "Atmosphere":
                    return "Атмосфера";
                case "Clear":
                    return "Ясно";
                case "Clouds":
                    return "Облачно";
                default:
                    return weatherMain;
            }
        }
        private void BackgroundVideo_Loaded(object sender, RoutedEventArgs e)
        {
            // Воспроизводим видео после загрузки элемента управления
            backgroundVideo.Play();
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Перезапуск видео с начала после окончания воспроизведения
            backgroundVideo.Position = TimeSpan.Zero;
            backgroundVideo.Play();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnMinimize":
                    WindowState = WindowState.Minimized;
                    break;
                case "btnClose":
                    Close();
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
