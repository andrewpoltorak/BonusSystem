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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
            {
                return "Заповніть усі поля!";
            }
            if (!int.TryParse(phoneNumber, out _))
            {
                return "Номер телефону має бути цілим числом";
            }
            if (!phoneNumber.StartsWith("0") || phoneNumber.Length != 10)
            {
                return "Номер телефону має бути у форматі 0NNNNNNNNN";
            }
            return string.Empty;
        }
    }
}
