using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InquiryService.Model
{
    public class RabbitClient
    {
        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel channel { get; set; }

        public void Register(String message)
        {
            QueueDeclareOk token = channel.QueueDeclare(queue: "Inquiry", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            var props = channel.CreateBasicProperties();
            props.DeliveryMode = 2;
            channel.BasicPublish(exchange: "", routingKey: "Inquiry", basicProperties: props, body: body);
        }

        public void Deregister()
        {
            this.connection.Close();
        }

        public RabbitClient(ConnectionFactory connectionFactory)
        {
            this.factory = connectionFactory;
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }
    }
}
