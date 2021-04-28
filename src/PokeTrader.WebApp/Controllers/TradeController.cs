using Microsoft.AspNetCore.Mvc;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Trader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeTrader.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradesController : Controller
    {
        private readonly ITrader _trader;
        private readonly IHistory<Trade> _history;

        public TradesController(ITrader trader, IHistory<Trade> history)
        {
            _trader = trader;
            _history = history;
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

        [HttpGet("history")]
        public IActionResult GetAsync()
        {
            var trades = _history.Get();
            return Ok(trades);
        }
    }
}
