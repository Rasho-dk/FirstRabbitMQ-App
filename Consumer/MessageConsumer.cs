using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class MessageConsumer
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;
        private readonly string _queueName;


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <param name="queueName">Defines the name of the queue from which messages will be consumed.</param>
        /// <param name="hostName">Defines the hostname of the RabbitMQ server. Defaults to "localhost".</param>
        public MessageConsumer(string queueName, string hostName = "localhost")
        {   
            _factory = new ConnectionFactory() { HostName = hostName };
            _queueName = queueName;
        }

        /// <summary>
        /// Initializes the message consumer by creating a connection and channel to the RabbitMQ server, and declaring the specified queue.
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
        /// Starts listening for messages on the specified queue. When a message is received, it is decoded and printed to the console.
        /// </summary>
        public async Task StartListeningAsync()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");
                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("Waiting for messages. Press any key to exit...");
        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

    }
}
