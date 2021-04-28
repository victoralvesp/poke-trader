using Microsoft.AspNetCore.Mvc;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Dto.Trader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeTrader.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : Controller
    {
        ITrader _trader;

        public TradeController(ITrader trader)
        {
            _trader = trader;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TradeInfoDto dto)
        {
            try
            {
                var trade = _trader.MakeTrade(dto.ToModel());
                return Ok(trade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("approve")]
        public IActionResult Post([FromBody] ApproveTradeContextDto dto)
        {
            try
            {
                if(!TryValidateModel(dto))
                {
                    BadRequest("invalid model");
                }
                var (first, second) = dto.ToModel();
                var info = _trader.Check(first, second);
                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
