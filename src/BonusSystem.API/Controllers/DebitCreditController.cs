using BonusSystem.Business.Services;
using BonusSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPatch("sum:decimal")]
        public async Task<decimal> Patch([FromRoute] string cardId, [FromQuery] decimal sum)
        {
            return await _service.CreateTransactAsync(cardId, sum);
        }
    }
}
