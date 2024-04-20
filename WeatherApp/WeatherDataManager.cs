using System.Net.NetworkInformation;

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
}