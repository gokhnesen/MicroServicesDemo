using MicroServicesDemo.DTOs;

namespace MicroServicesDemo.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(platform),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Platform sent to Command service successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send platform to Command service.");
            }
        }
    }
}
