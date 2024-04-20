    using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WeatherApp.Utils
{
    public class TimeOfDayToVideoConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;

            // Определяем время суток: утро, день, вечер, ночь
            if (hour >= 6 && hour < 12)
            {
                return new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Videos\\morning_video.mp4", UriKind.RelativeOrAbsolute); // Путь к видео для утра
            }
            else if (hour >= 12 && hour < 18)
            {
                return new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Videos\\day_video.mp4", UriKind.RelativeOrAbsolute); // Путь к видео для дня
            }
            else if (hour >= 18 && hour < 22)
            {
                return new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Videos\\evening_video.mp4", UriKind.RelativeOrAbsolute); // Путь к видео для вечера
            }
            else
            {
                return new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Videos\\night_video.mp4", UriKind.RelativeOrAbsolute); // Путь к видео для ночи
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
