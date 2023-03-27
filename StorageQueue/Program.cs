using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using StorageQueue;
using System.Text.Json;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=gabundisaz204storage;AccountKey=aRI527R1WOiPSRa4Y5cluEyvucaG46kfbFZ5VrC+Sg+XLrUk2i00fTWigwfYpVB/D1kLH1QyCEfS+AStARqsuA==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";

//await SendMessage("O1", 100);
//await SendMessage("O2", 200);

await PeekMessage();

//await ReceiveMessage();

//Console.WriteLine($"The number of message in the queue is {GetQueueLength().Result}");

async Task SendMessage(string orderId, int quantity)
{
    QueueClient queueClient = new(connectionString, queueName);

    if (queueClient.Exists())
    {
        Order order = new() { OrderID = orderId, Quantity = quantity };

        await queueClient.SendMessageAsync(JsonSerializer.Serialize<Order>(order));
        Console.WriteLine("The order information has been sent");
    }
}

async Task PeekMessage()
{
    QueueClient queueClient = new(connectionString, queueName);
    int maxMessage = 10;

    PeekedMessage[] peekMessages = await queueClient.PeekMessagesAsync(maxMessage);
    Console.WriteLine("The messages in the queue are:");

    foreach (var peekMessage in peekMessages)
    {
        Order order = JsonSerializer.Deserialize<Order>(peekMessage.Body.ToString());
        Console.WriteLine($"Order Id {order.OrderID}");
        Console.WriteLine($"Quantity {order.Quantity}");
    }
}

async Task ReceiveMessage()
{
    QueueClient queueClient = new(connectionString, queueName);
    int maxMessage = 10;

    QueueMessage[] queueMessages = await queueClient.ReceiveMessagesAsync(maxMessage);
    Console.WriteLine("The messages in the queue are:");

    foreach (var message in queueMessages)
    {
        Console.WriteLine(message.Body);
        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
    }
}

async Task<int> GetQueueLength()
{
    QueueClient queueClient = new(connectionString, queueName);

    if (queueClient.Exists())
    {
        QueueProperties properties = await queueClient.GetPropertiesAsync();
        return properties.ApproximateMessagesCount;
    }

    return 0;
}