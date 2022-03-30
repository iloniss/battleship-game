using Battleship.CORE;
using Battleship.CORE.Helpers;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Battleship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly IBattleship _battleshipRepository;
        private readonly Converter _converter;

        public BattleshipController(IBattleship battleshipRepository)
        {
            _battleshipRepository = battleshipRepository;
            _converter = new Converter(10);
        }

        [HttpGet("playersBoards")]
        public ActionResult<StartBattleshipDTO> GetPlayersBoard()
        {
            var playersBoards = _battleshipRepository.GenerateGame();

            return Ok(new StartBattleshipDTO() {
                FirstPlayer = _converter.TransformToList(playersBoards[0]),
                SecondPlayer = _converter.TransformToList(playersBoards[1]) });
        }

        [HttpPost("movement")]
        public ActionResult<OutputBattleshipDTO> Movement([FromBody]List<List<int>> board)
        {
           var result = _battleshipRepository.Movement(_converter.TransformToArray(board));

            return Ok(new OutputBattleshipDTO() { 
            Board = _converter.TransformToList(result.Board),
            StatusGame = result.StatusGame});
        }
    }
}
