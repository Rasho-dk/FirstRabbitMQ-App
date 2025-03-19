using System;
using System.Text;
using RabbitMQ.Client;


// Create a connection to the RabbitMQ server
var factory = new ConnectionFactory() { HostName = "localhost" };

//new connection

  var conn = await factory.CreateConnectionAsync();

//new channel

var channel = await conn.CreateChannelAsync();

// Declare a queue
await channel.QueueDeclareAsync(
    queue: "letterbox",
    durable: false,
     exclusive: false,
     autoDelete: false,
     arguments: null
);

// Create a message

string message = "This is my first Message!";
// Convert the message to a byte array (encode)
var encodedMessage = Encoding.UTF8.GetBytes(message);

// publish the message to the queue
await channel.BasicPublishAsync(
    exchange: "",
    routingKey: "letterbox",
    mandatory: false,
    basicProperties: new BasicProperties(), // Specify the type argument explicitly
    body: encodedMessage
);


Console.WriteLine($"Published message: {message}");