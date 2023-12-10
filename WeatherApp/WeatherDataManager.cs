using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace WeatherApp
{
    public class WeatherDataManager
    {
        public bool CheckInternetConnection()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("8.8.8.8"); // Google Public DNS
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }

        public void ShowNoInternetMessage()
        {
            var noInternetWindow = new NoInternetWindow();
            noInternetWindow.ShowDialog(); // Показать окно с сообщением об отсутствии интернета
        }
    }
}
