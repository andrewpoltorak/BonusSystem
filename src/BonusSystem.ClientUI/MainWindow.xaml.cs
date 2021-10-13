using BonusSystem.ClientUI.Business.Validations;
using BonusSystem.ClientUI.Models.RequestModels;
using BonusSystem.ClientUI.Models.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BonusSystem.ClientUI
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly string baseUri = "http://localhost:5000/api";

        private BonusCardResponseModel bonusCardInfo = new BonusCardResponseModel();

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

        #region Search
        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
            string searchString = TextBoxSearch.Text;
            string validationMessage = SearchValidator.ValidationSearchValue(searchString, RadioButTelNumber.IsChecked.Value);
            if (validationMessage.Length > 0)
            {
                TextBlockErrorInfo.Text = validationMessage;
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
                        LabelCard.Content = response.CardNumber;
                        LabelDateStart.Content = response.CardDateStart.Date;
                        LabelDateEnd.Content = response.CardDateEnd.Date;
                        LabelSum.Content = response.Sum;
                    }
                    else
                    {
                        TextBlockErrorInfo.Text = "Інформацію по введеним даним пошуку не знайдено";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                };
            }
        }

        public async Task<BonusCardResponseModel> GetAllInfo(string searchValue)
        {
            HttpResponseMessage response = await client.GetAsync($"{baseUri}/BonusCard/{searchValue}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                bonusCardInfo = JsonConvert.DeserializeObject<BonusCardResponseModel>(result);
                response.Dispose();
            }
            else
            {
                TextBlockErrorInfo.Text = $"У процесі запиту сталася помилка. Статус код: {response.StatusCode}";
            }
            return bonusCardInfo;
        }
        #endregion

        #region DebitCredit
        private async void ButtonTransact_Click(object sender, RoutedEventArgs e)
        {
            var isNegative = TextBoxTransact.Text.StartsWith("-");

            string valueString = CreateValidString(isNegative);

            string validationMessage = TransactValidator.ValidationTransactValue(valueString, bonusCardInfo);
            if (validationMessage.Length > 0)
            {
                TextBlockErrorInfo.Text = validationMessage;
            }
            else
            {
                try
                {
                    valueString = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", decimal.Parse(valueString, CultureInfo.InvariantCulture));
                    decimal response = await CreateTransact(valueString, isNegative);
                    LabelSum.Content = response;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                };
            }
        }

        private string CreateValidString(bool isNegative)
        {
            string valueString = TextBoxTransact.Text.Replace(',', '.');
            if (isNegative)
            {
                var regex = new Regex(Regex.Escape("-"));
                valueString = regex.Replace(valueString, "", 1);
            }

            return valueString;
        }

        private async Task<decimal> CreateTransact(string value, bool isNegative)
        {
            value = isNegative ? value.Insert(0, "-") : value;
            decimal responseValue;
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{baseUri}/DebitCredit/{bonusCardInfo.CardId}/{value}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                responseValue = JsonConvert.DeserializeObject<decimal>(result);
                response.Dispose();
            }
            else
            {
                TextBlockErrorInfo.Text = $"У процесі запиту сталася помилка: {response.StatusCode}";
                responseValue = bonusCardInfo.Sum;
            }
            return responseValue;
        }
        #endregion

        #region Create
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBoxInputName.Text;
            string phoneNumber = TextBoxInputTel.Text;
            string validationMessage = CreateValidator.ValidationCreateValues(name, phoneNumber);
            if (validationMessage.Length > 0)
            {
                TextBlockErrorCreate.Text = validationMessage;
            }
            else
            {
                try
                {
                    CreateClientAndCardRequestModel model = new CreateClientAndCardRequestModel()
                    {
                        Name = name,
                        Telephone = phoneNumber
                    };
                    BonusCardResponseModel response = await CreateClientAndCard(model);
                    if (response.CardId is not null)
                    {
                        TextBlockErrorCreate.Text = "";
                        MessageBox.Show($"Кліент {name} створений", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                };
            }
        }

        private async Task<BonusCardResponseModel> CreateClientAndCard(CreateClientAndCardRequestModel model)
        {
            BonusCardResponseModel newClient = new BonusCardResponseModel();
            HttpResponseMessage response = await client.PostAsJsonAsync($"{baseUri}/BonusCard", model).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                newClient = JsonConvert.DeserializeObject<BonusCardResponseModel>(result);
                response.Dispose();
            }
            else
            {
                TextBlockErrorInfo.Text = $"У процесі запиту сталася помилка. Статус код: {response.StatusCode}";
            }
            return newClient;
        }
        #endregion

        #region ClearMethods
        private void ClearAllFields()
        {
            TextBlockErrorInfo.Text = "";
            LabelName.Content = "";
            LabelTel.Content = "";
            LabelCard.Content = "";
            LabelDateStart.Content = "";
            LabelDateEnd.Content = "";
            LabelSum.Content = "";
            TextBoxTransact.Text = "";
        }

        private void ClearAllFieldsOnCreateTab()
        {
            TextBlockErrorCreate.Text = "";
            TextBoxInputName.Text = "";
            TextBoxInputTel.Text = "";
        }

        private void TabControlMain_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TabItemCreate is not null && TabItemCreate.IsSelected)
            {
                ClearAllFieldsOnCreateTab();
            }
            if (TabItemInfo is not null && TabItemInfo.IsSelected && TextBlockErrorInfo is not null)
            {
                TextBlockErrorInfo.Text = "";
            }
        }
        #endregion
    }
}
