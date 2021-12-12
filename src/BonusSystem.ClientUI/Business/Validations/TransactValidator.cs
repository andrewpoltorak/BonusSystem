using BonusSystem.ClientUI.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusSystem.ClientUI.Business.Validations
{
    public class TransactValidator
    {
        public static string ValidationTransactValue(string value, BonusCardResponseModel bonusCardInfo)
        {
            if (bonusCardInfo.CardId is null)
            {
                return "Щоб внести чи списати суму спочатку знайдіть необхідну картку";
            }
            if (string.IsNullOrEmpty(value))
            {
                return "Неможливо внести чи списати пусте значення";
            }
            if (!double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _) || decimal.Parse(value, CultureInfo.InvariantCulture) == 0)
            {
                return "Сума внесення чи списання має бути числом і не дорівнювати нулю";
            }
            if (value.Any(char.IsWhiteSpace))
            {
                return "Сума внестення чи списання не має містити пробіл";
            }
            return string.Empty;
        }
    }
}
