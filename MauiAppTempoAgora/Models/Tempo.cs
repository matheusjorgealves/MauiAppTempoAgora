namespace MauiAppTempoAgora.Models
{
    // dados do tempo que serão recebidos da API OpenWeatherMap
    public class Tempo
    {
        public double? lon { get; set; } // longitude
        public double? lat { get; set; } // latitude
        public double? temp_min { get; set; } // temperatura mínima
        public double? temp_max { get; set; } // temperatura máxima
        public int? visibility { get; set; } // visibilidade
        public double? speed { get; set; } // velocidade do vento
        public string? main { get; set; } // condição principal do tempo
        public string? description { get; set; } // descrição detalhada do tempo
        public string? sunrise { get; set; } // horário do nascer do sol
        public string? sunset { get; set; } // horário do pôr do sol
    }
}
