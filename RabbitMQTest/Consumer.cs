using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using RabbitMQExample.Utils;

namespace RabbitMQExample
{
    public class Consumer
    {
        public void ProcessTaskFromQueue()
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
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: ConfigurationReader.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var taskParamsObject = Newtonsoft.Json.JsonConvert.DeserializeObject
                            <TaskParams>(Encoding.UTF8.GetString(ea.Body));

                        var type = Type.GetType(taskParamsObject.ClassName);
                        if (type == null)
                        {
                            var temp = taskParamsObject.ClassName.Split('.');
                            type = Assembly.Load(temp[0]).GetTypes().First(t => t.Name == temp[1]);
                        }

                        var tempObject = Activator.CreateInstance(type);
                        var method = tempObject.GetType().GetMethod(taskParamsObject.MethodName);
                        method.Invoke(tempObject, taskParamsObject.Parameters ?? null);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };
                    channel.BasicConsume(queue: ConfigurationReader.QueueName, noAck: true, consumer: consumer);
                    Console.ReadLine();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Following Error Occured: {0}", ex);
            }
        }
    }
}
