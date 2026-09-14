using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // evento de clique do botão
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if(t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat}\n" +
                                         $"Longitude: {t.lon}\n" +
                                         $"Descrição: {t.description}\n" +
                                         $"Velocidade do Vento: {t.speed} m/s\n" +
                                         $"Visibilidade: {t.visibility} m\n" +
                                         $"Nascer do Sol: {t.sunrise}\n" +
                                         $"Pôr do Sol: {t.sunset}\n" +
                                         $"Temp Máx: {t.temp_max}\n" +
                                         $"Temp Min: {t.temp_min}\n";

                        lbl_res.Text = dados_previsao;

                    } else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }

                } else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }

            } catch (HttpRequestException) // tratamento de erro de conexão
            {
                await DisplayAlertAsync("Sem conexão",
                        "Não foi possível conectar à internet.",
                        "OK");
            } catch(Exception ex) // tratamento de erro genérico
            {
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }
    }
}
