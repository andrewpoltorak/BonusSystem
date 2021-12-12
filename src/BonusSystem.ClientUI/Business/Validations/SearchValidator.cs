namespace BonusSystem.ClientUI.Business.Validations
{
    public class SearchValidator
    {
        public static string ValidationSearchValue(string searchValue, bool isPhoneNumber)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return "Пошукові параметри не можуть бути пустими";
            }
            if (!int.TryParse(searchValue, out _))
            {
                return "Пошукові параметри мають бути цілим числом";
            }
            if (isPhoneNumber)
            {
                if (!searchValue.StartsWith("0") || searchValue.Length != 10)
                {
                    return "Номер телефону має бути у форматі 0NNNNNNNNN";
                }
            }
            else if (searchValue.Length > 6)
            {
                return "Номер картки не має перевищувати 6 символів";
            }
            return string.Empty;
        }
    }
}
