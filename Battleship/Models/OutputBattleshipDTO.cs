using System.Collections.Generic;

namespace Battleship.Models
{
    public class OutputBattleshipDTO
    {
        public List<List<int>> Board { get; set; }
        public int StatusGame { get; set; }
    }
}
