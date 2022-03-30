using System.Collections.Generic;

namespace Battleship.CORE.Helpers
{
    public class Converter
    {
        private readonly int _size;

        public Converter(int size)
        {   
            _size = size;
        }

        public int[,] TransformToArray(List<List<int>> board)
        {
            int[,] array = new int[_size, _size];

            for (int k = 0; k < _size; k++)
            {
                for (int j = 0; j < _size; j++)
                {
                    array[k, j] = board[k][j];
                }
            }
            return array;
        }

        public List<List<int>> TransformToList(int[,] board)
        {
            List<List<int>> iList = new();

            for (int k = 0; k < _size; k++)
            {
                List<int> jList = new();
                for (int j = 0; j < _size; j++)
                {
                    jList.Add(board[k, j]);
                }
                iList.Add(jList);
            }
            return iList;
        }
    }
}
