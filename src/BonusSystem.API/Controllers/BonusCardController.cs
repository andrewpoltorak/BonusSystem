using BonusSystem.Business.Services;
using BonusSystem.Models.Entities;
using BonusSystem.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BonusSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusCardController : ControllerBase
    {
        private readonly IBonusCardService _service;

        public BonusCardController(IBonusCardService service)
        {
            _service = service;
        }

        [HttpGet("{searchValue}")]
        public async Task<BonusCard> Get(string searchValue)
        {
            BonusCard bonusCard = null;
            if (!string.IsNullOrEmpty(searchValue))
            {
                bonusCard = await _service.GetByValueAsync(searchValue);
            }
            return bonusCard;
        }

        [HttpPost]
        public async Task<BonusCard> Post([FromBody] CreateClientAndCardRequestModel requestModel)
        {
            if (!string.IsNullOrEmpty(requestModel.Name) && !string.IsNullOrEmpty(requestModel.Telephone))
            {
                return await _service.CreateCardAsync(requestModel);
            }
            return null;
        }
    }
}
