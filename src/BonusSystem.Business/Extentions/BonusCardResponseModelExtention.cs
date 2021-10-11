using BonusSystem.Models.Entities;
using BonusSystem.Models.ResponseModels;

namespace BonusSystem.Business.Extentions
{
    public static class BonusCardResponseModelExtention
    {
        public static BonusCardResponseModel ToResponseModel(this BonusCardResponseModel model, BonusCard bonusCard, decimal bonusSum, Client client)
        {
            model.CardId = bonusCard.Id;
            model.CardNumber = bonusCard.Number;
            model.UserName = client.Name;
            model.UserPhoneNumber = client.PhoneNumber;
            model.Sum = bonusSum;
            model.CardDateStart = bonusCard.DateStart;
            model.CardDateEnd = bonusCard.DateEnd;
            return model;
        }
    }
}
