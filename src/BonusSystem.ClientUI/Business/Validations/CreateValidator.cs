using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusSystem.ClientUI.Business.Validations
{
    public class CreateValidator
    {
        public static string ValidationCreateValues(string name, string phoneNumber)
        {
            string validationMassage = string.Empty;
            do
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
                {
                    validationMassage = "Заповніть усі поля!";
                    break;
                }
                if (!int.TryParse(phoneNumber, out _))
                {
                    validationMassage = "Номер телефону має бути цілим числом";
                    break;
                }
                if (!phoneNumber.StartsWith("0") || phoneNumber.Length != 10 )
                {
                    validationMassage = "Номер телефону має бути у форматі 0NNNNNNNNN";
                    break;
                }

            } while (false);
            return validationMassage;
        }
    }
}
