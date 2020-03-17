using System.Text.Json.Serialization;

namespace Wiz.Template.Function.Models
{
    public class ViaCEP
    {
        [JsonPropertyName("cep")]
        public string CEP { get; set; }
        [JsonPropertyName("logradouro")]
        public string Street { get; set; }
        [JsonPropertyName("complemento")]
        public string StreetFull { get; set; }
        [JsonPropertyName("uf")]
        public string UF { get; set; }

        public static string CEPFormat(string cep) => cep.Replace("-", string.Empty);
    }
}