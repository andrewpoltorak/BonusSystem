using BonusSystem.ClientUI.Business.Validations.Search;
using BonusSystem.ClientUI.Models.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace BonusSystem.ClientUI
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly string baseUri = "http://localhost:5000/api";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BonusSystemWindow_Loaded(object sender, RoutedEventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(1000000));
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();            
            string searchString = TextBoxSearch.Text;
            string validationMessage = SearchValidator.ValidationSearchValue(searchString, RadioButTelNumber.IsChecked.Value);
            if (validationMessage.Length > 0)
            {
                TextBlockErrorSearch.Text = validationMessage;
            }
            else
            {
                try
                {
                    var response = await GetAllInfo(TextBoxSearch.Text);
                    if (response.CardId is not null)
                    {
                        LabelName.Content = response.UserName;
                        LabelTel.Content = response.UserPhoneNumber;
                        LabelCard.Content = response.CardId;
                        LabelDateStart.Content = response.CardDateStart;
                        LabelDateEnd.Content = response.CardDateEnd;
                        LabelSum.Content = response.Sum;
                    }
                    else
                    {
                        TextBlockErrorSearch.Text = "Інформацію по введеним даним пошуку не знайдено";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                };
            }
        }

        private void ClearAllFields()
        {
            TextBlockErrorSearch.Text = "";
            LabelName.Content = "";
            LabelTel.Content = "";
            LabelCard.Content = "";
            LabelDateStart.Content = "";
            LabelDateEnd.Content = "";
            LabelSum.Content = "";
        }

        public async Task<BonusCardResponseModel> GetAllInfo(string searchValue)
        {
            BonusCardResponseModel bonusCardInfo = new BonusCardResponseModel();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await client.GetAsync($"{baseUri}/BonusCard/{searchValue}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                bonusCardInfo = JsonConvert.DeserializeObject<BonusCardResponseModel>(result);
                response.Dispose();
            }
            else
            {
                TextBlockErrorSearch.Text = $"У процесі запиту сталася помилка. Статус код: {response.StatusCode}";
            }
            return bonusCardInfo;
        }
    }
}
