using Newtonsoft.Json;
using System.Text;

namespace WebApplicationTestTask
{
    public class Client1
    {
        static async Task Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var message = new { Text = "Hello World", Order = 1 };
                var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:5001/api/messages", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Message sent successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to send message.");
                }
            }
        }
    }
}
