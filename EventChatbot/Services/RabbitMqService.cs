using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EventChatbot.Models;
using System.Web;


public class RabbitMqService
{
    private readonly string _hostName;
    private readonly string _queueName;
    private readonly string _responseQueueName;

    public RabbitMqService(IConfiguration configuration)
    {
        _hostName = configuration["RabbitMQ:HostName"];
        _queueName = configuration["RabbitMQ:QueueName"];
        _responseQueueName = configuration["RabbitMQ:ResponseQueueName"];
    }

    public async Task SendMessageAsync(string prompt, Stack<Message> conversationHistory)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        {
            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = new { prompt = prompt, conversation_history = conversationHistory };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
        }
    }

    public async void ListenForResponse(Action<string> onResponse)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using (var connection = await factory.CreateConnectionAsync())
        using (var channel = await connection.CreateChannelAsync())
        {
            await channel.QueueDeclareAsync(queue: _responseQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                 

                var decodedResponse = System.Text.RegularExpressions.Regex.Unescape(response);
                decodedResponse = decodedResponse.Replace("\n", "<br>");

                onResponse(decodedResponse); // Trigger the callback with the response
                return Task.CompletedTask;
            };

            channel.BasicConsumeAsync(queue: _responseQueueName, autoAck: true, consumer: consumer);

            // Keep the listener running
            Console.WriteLine("Listening for responses...");
            Console.ReadLine(); // This is a blocking call, stop when user presses Enter
        }
    }
}
