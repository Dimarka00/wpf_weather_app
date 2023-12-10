using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WeatherApp.Utils
{
    public class TimeOfDayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;

            // Определяем время суток: утро, день, вечер, ночь
            if (hour >= 6 && hour < 12)
            {
                return new BitmapImage(new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Images\\morning_image.jpg", UriKind.RelativeOrAbsolute)); // Путь к изображению для утра
            }
            else if (hour >= 12 && hour < 18)
            {
                return new BitmapImage(new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Images\\day_image.jpg", UriKind.RelativeOrAbsolute)); // Путь к изображению для дня
            }
            else if (hour >= 18 && hour < 22)
            {
                return new BitmapImage(new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Images\\evening_image.jpg", UriKind.RelativeOrAbsolute)); // Путь к изображению для вечера
            }
            else
            {
                return new BitmapImage(new Uri("C:\\Users\\Dima\\source\\repos\\WeatherApp\\WeatherApp\\Images\\night_image.jpg", UriKind.RelativeOrAbsolute)); // Путь к изображению для ночи
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
