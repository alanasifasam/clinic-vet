using clinic_vet.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Azure;
using System.Text;
using System.Text.Json;

namespace clinic_vet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EligibilityController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly string connectionString;
        public IQueueClient queueClient;

        public EligibilityController(IConfiguration config)
        {
            this.config = config;
            connectionString = this.config.GetValue<string>("AzureServiceBus");
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EligibilityModel model)
        {
            await SendMessageQueue(model);
            return Ok(model);
        }


        private async Task SendMessageQueue([FromBody] EligibilityModel model)
        {
            string queueName = "eligibility";
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonSerializer.Serialize(model);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
    }
}
