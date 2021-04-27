using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerManager _playerManager;

        public PlayerController(ILogger<PlayerController> logger, IPlayerManager PlayerManager)
        {
            _logger = logger;
            _playerManager = PlayerManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var players = await _playerManager.Get();
            return Ok(players);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync(string name)
        {
            var player = await _playerManager.Get(name);
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]PlayerDto dto)
        {
            try
            {
                await _playerManager.Add(dto.ToModel());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
