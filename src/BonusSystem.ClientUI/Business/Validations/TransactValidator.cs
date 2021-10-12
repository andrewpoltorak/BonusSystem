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
            string validationMassage = string.Empty;
            do
            {
                if (bonusCardInfo.CardId is null)
                {
                    validationMassage = "Щоб внести чи списати суму спочатку знайдіть необхідну картку";
                    break;
                }
                if (string.IsNullOrEmpty(value))
                {
                    validationMassage = "Неможливо внести чи списати пусте значення";
                    break;
                }
                if (!double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _) || decimal.Parse(value, CultureInfo.InvariantCulture) == 0)
                {
                    validationMassage = "Сума внесення чи списання має бути числом і не дорівнювати нулю";
                    break;
                }
                if (value.Any(char.IsWhiteSpace))
                {
                    validationMassage = "Сума внестення чи списання не має містити пробіл";
                    break;
                }
            } while (false);
            return validationMassage;
        }
    }
}
