using BonusSystem.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BonusSystem.Controllers
{
    [Route("api/[controller]/{cardId}")]
    [ApiController]
    public class DebitCreditController : ControllerBase
    {
        private readonly IDebitCreditService _service;

        public DebitCreditController(IDebitCreditService service)
        {
            _service = service;
        }

        [HttpPatch("{sum}")]
        public async Task<decimal> Patch([FromRoute] string cardId, string sum)
        {
            return await _service.CreateTransactAsync(cardId, sum);
        }
    }
}
