using System.Configuration;

namespace RabbitMQExample.Utils
{
    public static class ConfigurationReader
    {
        public static string ServerAddress = ConfigurationManager.AppSettings["ServerAddress"];
        public static string UserName = ConfigurationManager.AppSettings["UserName"];
        public static string Password = ConfigurationManager.AppSettings["Password"];
        public static string QueueName = ConfigurationManager.AppSettings["QueueName"];
    }
}
