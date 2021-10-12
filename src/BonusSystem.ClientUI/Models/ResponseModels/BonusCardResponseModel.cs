using System;

namespace BonusSystem.ClientUI.Models.ResponseModels
{
    public class BonusCardResponseModel
    {
        public string CardId { get; set; }

        public int CardNumber { get; set; }

        public string UserName { get; set; }

        public string UserPhoneNumber { get; set; }

        public decimal Sum { get; set; }

        public DateTime CardDateStart { get; set; }

        public DateTime CardDateEnd { get; set; }
    }
}
