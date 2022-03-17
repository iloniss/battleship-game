using Battleship.CORE;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly IBattleship _battleshipRepository;

        public BattleshipController(IBattleship battleshipRepository)
        {
            _battleshipRepository = battleshipRepository;
        }

        [HttpGet("playersBoards")]
        public async Task<ActionResult<StartBattleshipDTO>> GetPlayersBoard()
        {
            var playersBoards = _battleshipRepository.GenerateGame();


            return Ok(new StartBattleshipDTO() {
                FirstPlayer = _battleshipRepository.TransformToList(playersBoards[0]),
                SecondPlayer = _battleshipRepository.TransformToList(playersBoards[1]) });

        }

        [HttpPost("movement")]
        public async Task<ActionResult<OutputBattleshipDTO>> Movement([FromBody]List<List<int>> board)
        {
           var result = _battleshipRepository.Movement(_battleshipRepository.TransformToArray(board));

            return Ok(new OutputBattleshipDTO() { 
            Board = _battleshipRepository.TransformToList(result.Board),
            StatusGame = result.StatusGame});
        }

    }

}
