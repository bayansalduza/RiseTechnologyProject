using MongoDB.Bson.IO;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace RiseTechnologyProject.DataAccess.RabbitMQExtensions
{
    public class RabbitMQExtensions
    {
        public void AddToQueue(int uUID)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "ReportQueue", durable: false, exclusive: false, autoDelete: true, arguments: null);
                string data = JsonConvert.SerializeObject(uUID);
                var body = Encoding.UTF8.GetBytes(data);
                channel.BasicPublish(exchange: "", routingKey: "ReportQueue", basicProperties: null, body: body);
            }
        }

    }
}
