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

        // quando o botão de previsão é clicado
        private async void Button_Clicked_Previsao(object sender, EventArgs e)
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

        // quando o botão de localização é clicado
        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                // solicita a localização com precisão média e tempo de 10 segundos
                GeolocationRequest request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10)
                );

                // obtém a localização atual do dispositivo
                Location? local = await Geolocation.Default.GetLocationAsync(request);

                // exibe os dados da localização no label
                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude}";

                    lbl_coords.Text = local_disp;

                    string mapa = $"https://embed.windy.com/embed.html?" +
                                  $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                                  $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                  $"&lat={local.Latitude.ToString().Replace(",", ".")}&lon={local.Longitude.ToString().Replace(",", ".")}";

                    wv_mapa.Source = mapa;

                    // pega nome da cidade que está nas coordenadas
                    GetCidade(local.Latitude, local.Longitude);

                } else
                {
                    lbl_coords.Text = "Nenhuma Localização";
                }

            }
            // erro dispositivo não suporta
            catch (FeatureNotSupportedException fnsex)
            {
                await DisplayAlertAsync("Erro: Dispositivo não suporta", fnsex.Message, "OK");
            }
            // erro recurso desabilitado
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlertAsync("Erro: Localização desabilitada", fneEx.Message, "OK");
            }
            // erro permissão negada
            catch (PermissionException pEx)
            {
                await DisplayAlertAsync("Erro: Permissão da Localização", pEx.Message, "OK");
            }
            // erro genérico
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }

        // nome da cidade a partir da latitude e longitude
        private async void GetCidade(double lat, double lon)
        {
            try
            {
                // lista de lugares a partir da latitude e longitude
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                // pega o primeiro lugar da lista ou null
                Placemark? place = places.FirstOrDefault();

                // se houver um lugar
                if (place != null)
                {
                    // exibe o nome da cidade no campo de texto
                    txt_cidade.Text = place.Locality;
                }
            } catch (Exception ex)
            {
                await DisplayAlertAsync("Erro: Obtenção do nome da Cidade", ex.Message, "OK");
            }
        }
    }
}
