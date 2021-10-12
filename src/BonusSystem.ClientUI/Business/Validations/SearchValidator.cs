namespace BonusSystem.ClientUI.Business.Validations
{
    public class SearchValidator
    {
        public static string ValidationSearchValue(string searchValue, bool isPhoneNumber)
        {
            string validationMassage = string.Empty;
            do
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    validationMassage = "Пошукові параметри не можуть бути пустими";
                    break;
                }
                if (!int.TryParse(searchValue, out _))
                {
                    validationMassage = "Пошукові параметри мають бути цілим числом";
                    break;
                }
                if (isPhoneNumber)
                {
                    if (!searchValue.StartsWith("0") || searchValue.Length != 10)
                    {
                        validationMassage = "Номер телефону має бути у форматі 0NNNNNNNNN";
                    }
                    break;
                }
                else if(searchValue.Length > 6)
                {
                    validationMassage = "Номер картки не має перевищувати 6 символів";
                    break;
                }
            } while (false);
            return validationMassage;
        }
    }
}
