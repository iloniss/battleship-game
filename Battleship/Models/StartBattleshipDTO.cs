using System.Collections.Generic;

namespace Battleship.Models
{
    public class StartBattleshipDTO
    {
        public List<List<int>> FirstPlayer { get; set; }
        public List<List<int>> SecondPlayer { get; set; }
    }
}
