using System;
using Battleship.CORE.Enumerations;
using System.Collections.Generic;
using System.Linq;
using Battleship.CORE.Models;

namespace Battleship.CORE

{  
    public interface IBattleship
    {
        List<int[,]> GenerateGame();
        MovementOutput Movement(int[,] board);
    }
    public class Battleship: IBattleship
    {
        private const int size = 10;

        public List<int[,]> GenerateGame()
        {
            List<int[,]> boardList = new();

            for (int i = 0; i < 2; i++)
            {
                int[,] board = CreateArray();

                List<int> sizeBattleship = new() { 5, 4, 3, 2, 2, 1, 1 };

                foreach (int s in sizeBattleship)
                {
                    board = FillArray(board, new List<Models.Index>(), s);
                }
                boardList.Add(board);
            }
            return boardList;
        }

        public MovementOutput Movement(int[,] board)
        {
            var availablePlayIndex = SavePlayIndex(board);
            var startPlayIndex = CreatePlayIndex(availablePlayIndex);
            int iPlayRandom = startPlayIndex.Row;
            int jPlayRandom = startPlayIndex.Column;
                    
            if (board[iPlayRandom, jPlayRandom] == (int)FieldEnum.BattleshipField)
                board[iPlayRandom, jPlayRandom] = (int)FieldEnum.HitField;
            else
            { 
                board[iPlayRandom, jPlayRandom] = (int)FieldEnum.UsedField;

                return new MovementOutput()
                {
                    Board = board,
                    StatusGame = (int)StatusEnum.Mishit
                };
            }       

            if (CheckStatusFieldBattleship(board))
            {
                return new MovementOutput()
                {
                    Board = board,
                    StatusGame = (int)StatusEnum.Hit
                };
            }
            else
            {
                return new MovementOutput()
                {
                    Board = board,
                    StatusGame = (int)StatusEnum.EndOfGame
                };
            }              
        }

        private static bool CheckStatusFieldBattleship(int[,] board)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == (int)FieldEnum.BattleshipField)
                        return true;
                }
            }
            return false;
        }

        private static Models.Index CreatePlayIndex(List<Models.Index> availablePlayIndex)
        {
            int id = new Random().Next(availablePlayIndex.Count);

            var startPlayIndex = availablePlayIndex[id];

            return startPlayIndex;
        }

        private static List<Models.Index> SavePlayIndex(int[,] board)
        {
            var availablePlayIndex = new List<Models.Index>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == (int)FieldEnum.EmptyField || board[i, j] == (int)FieldEnum.ForbiddenField || board[i, j] == (int)FieldEnum.BattleshipField)
                    {
                        availablePlayIndex.Add(new Models.Index()
                        {
                            Row = i,
                            Column = j
                        });
                    }
                }
            }
            return availablePlayIndex;
        }

        private static int[,] CreateArray()
        {
            int[,] board = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = (int)FieldEnum.EmptyField;
                }
            }
            return board;
        }

        private int[,] FillArray(int[,] board, List<Models.Index> availableIndex, int sizeBattleship)
        {
            if (availableIndex.Count == 0)
                availableIndex = SaveIndex(board);
            var startIndex = CreateIndex(availableIndex);
            int iRandom = startIndex.Row;
            int jRandom = startIndex.Column;
            int direction = new Random().Next(3) + 1;
            List<int> checkedDirections = new();


            board = GenerateShip(iRandom, jRandom, direction, sizeBattleship, board, checkedDirections, availableIndex);

            return board;
        }

        private int[,] GenerateShip(int iRandom, int jRandom, int direction, int sizeBattleship,
            int[,] board, List<int> checkedDirections, List<Models.Index> availableIndex)
        {
            if (CheckDirection(direction, iRandom, jRandom, sizeBattleship, board))
            {
                board = CreateShip(jRandom, iRandom, sizeBattleship, board, direction);
                board = CreateForbiddenFields(board);
                return board;
            }
            else
            {
                if (checkedDirections.Count == 4)
                {
                    availableIndex = UpdateIndex(availableIndex, iRandom, jRandom);
                    return FillArray(board, availableIndex, sizeBattleship);
                }
                checkedDirections.Add(direction);

                direction++;

                if (direction > 4)
                    direction = 1;

                GenerateShip(iRandom, jRandom, direction, sizeBattleship, board, checkedDirections, availableIndex);
            }
            return board;
        }

        private static bool CheckDirection(int direction, int iRandom, int jRandom, int sizeBattleship, int[,] board)
        {
            switch (direction)
            {
                case (int)DirectionEnum.VerticalUp:
                    if (jRandom + 1 - sizeBattleship >= 0)
                        if (CheckStatusFieldUp(jRandom, iRandom, board, sizeBattleship))
                            return true;
                    return false;
                case (int)DirectionEnum.VerticalDown:
                    if (jRandom + 1 + sizeBattleship <= size)
                        if (CheckStatusFieldDown(jRandom, iRandom, board, sizeBattleship))
                            return true;
                    return false;
                case (int)DirectionEnum.HorizontalRight:
                    if (iRandom + 1 + sizeBattleship <= size)
                        if (CheckStatusFieldRight(jRandom, iRandom, board, sizeBattleship))
                            return true;
                    return false;
                case (int)DirectionEnum.HorizontalLeft:
                    if (iRandom + 1 - sizeBattleship >= 0)
                        if (CheckStatusFieldLeft(jRandom, iRandom, board, sizeBattleship))
                            return true;
                    return false;
                default:
                    return false;
            }
        }

        private static bool CheckStatusFieldUp(int jRandom, int iRandom, int[,] board, int sizeBattleship)
        {
            var correctField = new List<int>();

            for (int j = 0; j < sizeBattleship; j++)
            {
                if (board[iRandom, jRandom - j] == (int)FieldEnum.EmptyField)
                {
                    correctField.Add(j);
                }
                else
                    return false;
            }
            if (correctField.Count == sizeBattleship)
                return true;
            return false;
        }

        private static bool CheckStatusFieldDown(int jRandom, int iRandom, int[,] board, int sizeBattleship)
        {
            var correctField = new List<int>();

            for (int j = 0; j < sizeBattleship; j++)
            {
                if (board[iRandom, jRandom + j] == (int)FieldEnum.EmptyField)
                {
                    correctField.Add(j);
                }
                else
                    return false;
            }
            if (correctField.Count == sizeBattleship)
                return true;
            return false;
        }

        private static bool CheckStatusFieldRight(int jRandom, int iRandom, int[,] board, int sizeBattleship)
        {
            var correctField = new List<int>();

            for (int i = 0; i < sizeBattleship; i++)
            {
                if (board[iRandom + i, jRandom] == (int)FieldEnum.EmptyField)
                {
                    correctField.Add(i);
                }
                else
                    return false;
            }
            if (correctField.Count == sizeBattleship)
                return true;
            return false;
        }

        private static bool CheckStatusFieldLeft(int jRandom, int iRandom, int[,] board, int sizeBattleship)
        {
            var correctField = new List<int>();

            for (int i = 0; i < sizeBattleship; i++)
            {
                if (board[iRandom - i, jRandom] == (int)FieldEnum.EmptyField)
                {
                    correctField.Add(i);
                }
                else
                    return false;
            }
            if (correctField.Count == sizeBattleship)
                return true;
            return false;
        }

        private static int[,] CreateShip(int jRandom, int iRandom, int sizeBattleship, int[,] board, int direction)
        {
            switch (direction)
            {
                case (int)DirectionEnum.VerticalUp:
                    for (int j = 0; j < sizeBattleship; j++)
                        board[iRandom, jRandom - j] = (int)FieldEnum.BattleshipField;
                    break;
                case (int)DirectionEnum.VerticalDown:
                    for (int j = 0; j < sizeBattleship; j++)
                        board[iRandom, jRandom + j] = (int)FieldEnum.BattleshipField;
                    break;
                case (int)DirectionEnum.HorizontalRight:
                    for (int i = 0; i < sizeBattleship; i++)
                        board[iRandom + i, jRandom] = (int)FieldEnum.BattleshipField;
                    break;
                case (int)DirectionEnum.HorizontalLeft:
                    for (int i = 0; i < sizeBattleship; i++)
                        board[iRandom - i, jRandom] = (int)FieldEnum.BattleshipField;
                    break;
                default:
                    return board;
            }
            return board;
        }

        private static int[,] CreateForbiddenFields(int[,] board)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i < size - 1 && board[i + 1, j] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (i > 0 && board[i - 1, j] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j < size - 1 && board[i, j + 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j > 0 && board[i, j - 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j > 0 && i > 0 && board[i - 1, j - 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j < size - 1 && i < size - 1 && board[i + 1, j + 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j > 0 && i < size - 1 && board[i + 1, j - 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                    else if (j < size - 1 && i > 0 && board[i - 1, j + 1] == (int)FieldEnum.BattleshipField && board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        board[i, j] = (int)FieldEnum.ForbiddenField;
                    }
                }
            }
            return board;
        }

        private static List<Models.Index> SaveIndex(int[,] board)
        {
            var availableIndex = new List<Models.Index>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == (int)FieldEnum.EmptyField)
                    {
                        availableIndex.Add(new Models.Index()
                        {
                            Row = i,
                            Column = j
                        });
                    }
                }
            }
            return availableIndex;
        }

        private static Models.Index CreateIndex(List<Models.Index> availableIndex)
        {
            int id = new Random().Next(availableIndex.Count);

            var startIndex = availableIndex[id];

            return startIndex;
        }

        private static List<Models.Index> UpdateIndex(List<Models.Index> availableIndex, int iRandom, int jRandom)
        {
            return availableIndex.Where(x => x.Row != iRandom && x.Column != jRandom).ToList();

        }
    }
}
