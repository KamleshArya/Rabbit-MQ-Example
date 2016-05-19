using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQExample.Utils;

namespace RabbitMQExample
{
    public class Producer
    {
        public string Input;

        public Producer(TaskParams taskObject)
        {
            Input = Newtonsoft.Json.JsonConvert.SerializeObject(taskObject);
        }
        public void InsertTaskIntoQueue()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = ConfigurationReader.ServerAddress,
                    UserName = ConfigurationReader.UserName,
                    Password = ConfigurationReader.Password,
                    VirtualHost = "/",
                    Protocol = Protocols.DefaultProtocol,
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    RequestedHeartbeat = 1000,
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var model = connection.CreateModel())
                    {
                        model.CreateBasicProperties().Persistent = true;
                        model.QueueDeclare(queue: ConfigurationReader.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        model.BasicPublish(exchange: string.Empty, routingKey: ConfigurationReader.QueueName, basicProperties: null, body: Encoding.UTF8.GetBytes(Input));
                    }
                }
            }
            catch (System.IO.EndOfStreamException ex)
            {
                Console.WriteLine("Following Error Occured: {0}", ex);
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine("Following Error Occured: {0}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Following Error Occured: {0}", ex);
            }
        }
    }
}
