using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class MessagePublisher
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;

        private readonly string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisher"/> class.
        /// </summary>
        /// <param name="queueName">Defines the name of the queue to which messages will be published.</param>
        /// <param name="hostName">Defines the hostname of the RabbitMQ server. Defaults to "localhost".</param>
        public MessagePublisher(string queueName, string hostName = "localhost")
        {
            _factory = new ConnectionFactory() { HostName = hostName };
            _queueName = queueName;
        }

        /// <summary>
        /// Initializes the message publisher by creating a connection and channel to the RabbitMQ server, and declaring the specified queue.
        /// </summary>
        public async Task InitializeAsync()
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        /// <summary>
        /// Publishes a message to the specified queue. The message is encoded as a UTF-8 byte array before being sent.
        /// <para>
        /// <b>Note:</b> The <paramref name="message"/> parameter is a string. If you want to send an object, you need to serialize it (e.g., to JSON) before calling this method.
        /// </para>
        /// <example>
        /// For example, to send a person object with name and id:
        /// <code>
        /// var person = new { Id = 1, Name = "Alice" };
        /// string json = System.Text.Json.JsonSerializer.Serialize(person);
        /// await publisher.PublishMessageAsync(json);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="message">Defines the message to be published. It should be a string.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task PublishMessageAsync(string message)
        {
            if (_channel == null)
            {
                throw new InvalidOperationException("Publisher is not initialized. Call InitializeAsync() first.");
            }

            var encodedMessage = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: _queueName,
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: encodedMessage
            );

            Console.WriteLine($"Published message: {message}");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }


    }
}
