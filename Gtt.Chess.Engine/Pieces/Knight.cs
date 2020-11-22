using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    internal class Knight : Piece
    {
        public Knight(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            int[][] positions = {
                new[] {1, 2},
                new[] {2, 1},
                new[] {2, -1},
                new[] {1, -2},
                new[] {-1, -2},
                new[] {-2, -1},
                new[] {-2, 1},
                new[] {-1, 2}
            };

            foreach (var p in positions)
            {
                var c = Board.GetRelativeCell(CurrentCell, p[0], p[1]);
                if (c == null)
                {
                    continue;
                }
                if (c.CurrentPiece == null)
                {
                    yield return c;
                }

                if (c.HasOpposingPiece(this))
                {
                    yield return c;
                }
            }
        }
    }
}
