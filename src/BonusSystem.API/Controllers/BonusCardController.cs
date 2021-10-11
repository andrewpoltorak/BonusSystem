using BonusSystem.Business.Services;
using BonusSystem.Models.RequestModels;
using BonusSystem.Models.ResponseModels;
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
        public async Task<BonusCardResponseModel> Get(string searchValue)
        {
            BonusCardResponseModel response = null;
            if (!string.IsNullOrEmpty(searchValue))
            {
                response = await _service.GetByValueAsync(searchValue);
            }
            return response;
        }

        [HttpPost]
        public async Task<BonusCardResponseModel> Post([FromBody] CreateClientAndCardRequestModel requestModel)
        {
            if (!string.IsNullOrEmpty(requestModel.Name) && !string.IsNullOrEmpty(requestModel.Telephone))
            {
                return await _service.CreateCardAsync(requestModel);
            }
            return null;
        }
    }
}
