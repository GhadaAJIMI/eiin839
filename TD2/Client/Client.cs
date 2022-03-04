using System.Text.Json;

static void ShowCount(Client.Service service)
{
    Console.WriteLine($"New Count: {service.count}");
}

Client.Service service = new Client.Service();

HttpClient client = new HttpClient();
string response = await client.GetStringAsync("http://localhost:8080/api/incr?param1=1%22);
service = JsonSerializer.Deserialize<Client.Service>(response);

ShowCount(service);

response = await client.GetStringAsync("http://localhost:8080/api/incr?param1=1%22);
service = JsonSerializer.Deserialize<Client.Service>(response);

ShowCount(service);

Console.ReadLine();