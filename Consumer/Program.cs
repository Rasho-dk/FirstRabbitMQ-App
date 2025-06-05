using Consumer;

var messageConsumer = new MessageConsumer(queueName:"test");
await messageConsumer.InitializeAsync();
await messageConsumer.StartListeningAsync();
// await messageConsumer.DisposeAsync();
Console.ReadKey();

#region Old
//using System;
//using System.Text;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//// run RabbitMQ server
//// docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management 
//// Create a connection to the RabbitMQ server
//var factory = new ConnectionFactory() { HostName = "localhost" };

//// new connection
//var conn = await factory.CreateConnectionAsync();

//// new channel
//var channel = await conn.CreateChannelAsync();

//// Declare a queue
//await channel.QueueDeclareAsync(
//    queue: "letterbox",
//    durable: false,
//    exclusive: false,
//    autoDelete: false,
//    arguments: null
//);

//var consumer = new AsyncEventingBasicConsumer(channel);

//// call back
//consumer.ReceivedAsync += (model, ea) =>
//{
//    var body = ea.Body.ToArray(); // Convert the message to a byte array (decode)
//    var message = Encoding.UTF8.GetString(body); // Decode the message
//    Console.WriteLine($"Received message: {message}"); // Display the message
//    return Task.CompletedTask;
//};

//await channel.BasicConsumeAsync(queue: "letterbox", autoAck: true, consumer: consumer);

//Console.WriteLine("Waiting for messages...");

//Console.ReadKey();

#endregion