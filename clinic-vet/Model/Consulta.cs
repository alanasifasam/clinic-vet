using Microsoft.Azure.Amqp.Framing;

namespace clinic_vet.Model
{
    [Serializable]
    public class Consulta
    {
        public int IdConsulta { get; set; }
        public bool Staus { get; set; }
        public DateTime DataConsulta { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int NumeroPlanoSaude { get; set; }
        public string? TipoConsulta { get; set; }
        public string? HistoricoDescricaoConsulta { get; set; }
        public int IdPaciente { get; set; }

        


      



    }
}
