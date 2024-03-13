using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory { HostName = "rabbitmq", UserName = "ccunha", Password = "#00Teste01" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.ConfirmSelect();

channel.QueueDeclare(queue: "testequeue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

const string message = $"  {{\r\n    \"_id\": \"65f0bec183d8da8be75971e2\",\r\n    \"index\": 0,\r\n    \"guid\": \"6db7f838-77bf-42d6-b224-b6399a58ec5b\",\r\n    \"isActive\": true,\r\n    \"balance\": \"$2,511.93\",\r\n    \"picture\": \"http://placehold.it/32x32\",\r\n    \"age\": 26,\r\n    \"eyeColor\": \"green\",\r\n    \"name\": \"Frazier Miller\",\r\n    \"gender\": \"male\",\r\n    \"company\": \"NORSUL\",\r\n    \"email\": \"fraziermiller@norsul.com\",\r\n    \"phone\": \"+1 (936) 440-2751\",\r\n    \"address\": \"831 Degraw Street, Allentown, Virginia, 3970\",\r\n    \"about\": \"Ullamco consequat nisi elit nostrud ullamco consectetur magna sunt magna labore minim elit. Irure dolore veniam non nisi exercitation proident ut aliquip Lorem occaecat deserunt. Eu ea ut culpa nostrud labore aliquip. Labore pariatur velit ad consequat officia proident minim ea duis aliqua elit et reprehenderit.\\r\\n\",\r\n    \"registered\": \"2023-01-09T12:49:01 -00:00\",\r\n    \"latitude\": 1.047149,\r\n    \"longitude\": -171.421765,\r\n    \"tags\": [\r\n      \"ea\",\r\n      \"sint\",\r\n      \"culpa\",\r\n      \"eiusmod\",\r\n      \"sunt\",\r\n      \"aliqua\",\r\n      \"ex\"\r\n    ],\r\n    \"friends\": [\r\n      {{\r\n        \"id\": 0,\r\n        \"name\": \"Anita Dickson\"\r\n      }},\r\n      {{\r\n        \"id\": 1,\r\n        \"name\": \"Rochelle Carlson\"\r\n      }},\r\n      {{\r\n        \"id\": 2,\r\n        \"name\": \"Wells Byers\"\r\n      }}\r\n    ],\r\n    \"greeting\": \"Hello, Frazier Miller! You have 6 unread messages.\",\r\n    \"favoriteFruit\": \"strawberry\"\r\n  }}";
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

Stopwatch sw = Stopwatch.StartNew();

Stopwatch sw2 = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    sw.Restart();
    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "testequeue",
                         basicProperties: properties,
                         body: body);

    channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
    sw.Stop();

    Console.WriteLine($"Sent a message in {sw.ElapsedMilliseconds} ms");
}

sw2.Stop();
Console.WriteLine($"Tempo total {sw2.ElapsedMilliseconds} ms");
