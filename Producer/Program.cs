using System.Text.Json;
using Consumer;

var publisher = new MessagePublisher(queueName:"test");
await publisher.InitializeAsync();

await publisher.PublishMessageAsync("Hello, RabbitMQ!");
await publisher.PublishMessageAsync("This is a test message.");

// publish custom object: 
var person = new Person(1,"John", "Doe");
var personSerializer = JsonSerializer.Serialize(person);
await publisher.PublishMessageAsync(personSerializer);

await publisher.PublishMessageAsync("Goodbye, RabbitMQ!");
Console.WriteLine("Messages published. Press any key to exit...");

Console.ReadKey();
//publisher.Dispose(); // To ensure resources are cleaned up

#region Old
//using System;
//using System.Text;
//using RabbitMQ.Client;


//// Create a connection to the RabbitMQ server
//var factory = new ConnectionFactory() { HostName = "localhost" };

////new connection

//var conn = await factory.CreateConnectionAsync();

////new channel

//var channel = await conn.CreateChannelAsync();

//// Declare a queue
//await channel.QueueDeclareAsync(
//    queue: "letterbox",
//    durable: false,
//     exclusive: false,
//     autoDelete: false,
//     arguments: null
//);

//// Create a message

//string message = "This is my first Message!";
//// Convert the message to a byte array (encode)
//var encodedMessage = Encoding.UTF8.GetBytes(message);

//// publish the message to the queue
//await channel.BasicPublishAsync(
//    exchange: "",
//    routingKey: "letterbox",
//    mandatory: false,
//    basicProperties: new BasicProperties(), // Specify the type argument explicitly
//    body: encodedMessage
//);


//Console.WriteLine($"Published message: {message}");


#endregion


public readonly record struct Person(int Id, string FirstName, string LastName);