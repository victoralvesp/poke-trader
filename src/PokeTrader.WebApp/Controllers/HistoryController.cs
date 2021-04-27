using Microsoft.AspNetCore.Mvc;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeTrader.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : Controller
    {
        IHistory<Trade> _history;

        public HistoryController(IHistory<Trade> history)
        {
            _history = history;
        }

        [HttpGet]
        public IActionResult GetAsync()
        {
            var history = _history.Get();
            return Ok(history);
        }
    }
}
