using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    public class Knight2 : Piece2
    {
        public Knight2(Color color, bool hasMoved) : base(color, "N", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
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
                var c = CurrentCell.Board.GetRelativeCell(CurrentCell, p[0], p[1]);
                if (c == null)
                {
                    continue;
                }
                if (c.Occupant == null)
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