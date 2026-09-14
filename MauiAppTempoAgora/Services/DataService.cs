using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        // método que retorna a previsão do tempo para uma determinada cidade
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null; // recebe a previsão do tempo

            string chave = "6d37adfd6f70c8dc9cd0c28e7cc2e099"; // chave API

            // URL da API
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            // faz a requisição HTTP para a API
            using (HttpClient client = new HttpClient())
            {
                // envia a requisição GET para a API
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // se a cidade não for encontrada, lança uma exceção
                    throw new Exception("Cidade não encontrada.");
                }

                if (resp.IsSuccessStatusCode) // se for bem sucedida
                {
                    // lê a resposta da API como string
                    string json = await resp.Content.ReadAsStringAsync();

                    // converte a string JSON em um objeto JObject
                    var rascunho = JObject.Parse(json);

                    // conversão do horário do nascer do sol e do pôr do sol para horário local
                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    // cria um objeto Tempo com os dados da previsão do tempo
                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; // fecha objeto do Tempo 
                } // fecha if se o status do servidor for de sucesso
            } // fecha laço using

            return t;
        }
    }
}
