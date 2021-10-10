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
    [Route("api/[controller]/{cardId:string}")]
    [ApiController]
    public class DebitCreditController : ControllerBase
    {
        private readonly IDebitCreditService _service;

        public DebitCreditController(IDebitCreditService service)
        {
            _service = service;
        }

        [HttpPatch("sum:int")]
        public async Task Patch([FromRoute] string cardId, [FromQuery] int sum)
        {
            await _service.CreateTransactAsync(cardId, sum);
        }
    }
}
