using clinic_vet.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Azure;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace clinic_vet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly string connectionString;
        
       

        public ConsultaController(IConfiguration config)
        {
            this.config = config;
            connectionString = this.config.GetValue<string>("AzureServiceBus");
            
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Consulta model)
        {

            if (model == null)
                return BadRequest();


            var eligibility = new EligibilityModel() 
            { 
                Id = 1,
                consulta = new Consulta()
                {
                    IdConsulta = model.IdConsulta,
                    IdPaciente = model.IdPaciente,
                    Staus = model.Staus,
                    DataConsulta = model.DataConsulta,
                    NumeroPlanoSaude = model.NumeroPlanoSaude,
                    HistoricoDescricaoConsulta = model.HistoricoDescricaoConsulta,
                }
            };
            await SendMessageQueue(eligibility);
            return Ok("Relatório da consulta enviado ao plano de saúde responsável.");
        }


        private async Task SendMessageQueue(EligibilityModel model)
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
