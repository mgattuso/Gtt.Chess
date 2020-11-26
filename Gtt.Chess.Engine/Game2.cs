using System;
using System.Collections.Generic;
using System.Linq;
using Gtt.Chess.Engine.Extensions;
using Gtt.Chess.Engine.Pieces;

namespace Gtt.Chess.Engine
{
    public class Game2
    {
        public GameStyle Style { get; }
        public Guid Id { get; }
        public Color CurrentTurn { get; protected set; }
        public Dictionary<Color, ICollection<Piece2>> Captures { get; }

        protected Game2()
        {
            Id = Guid.NewGuid();
            Captures = new Dictionary<Color, ICollection<Piece2>>
            {
                [Color.White] = new List<Piece2>(),
                [Color.Black] = new List<Piece2>()
            };

        }

        public Game2(GameStyle style) : this()
        {
            Style = style;
            switch (style)
            {
                case GameStyle.Traditional:
                    Board = new Board2(this);
                    PlaceTraditionalPieces(Board);
                    CurrentTurn = Color.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        protected Game2(GameStyle style, Board2 copyFrom, Color nextTurn) : this()
        {
            Style = style;
            switch (style)
            {
                case GameStyle.Traditional:
                    Board = new Board2(this);
                    CopyBoard(copyFrom);
                    CurrentTurn = nextTurn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        private void CopyBoard(Board2 copyFrom)
        {
            foreach (var cell in copyFrom.Cells)
            {
                if (cell.IsOccupied())
                {
                    Board.AddPiece(
                        PieceFactory.FromCode(cell.Occupant.Code, cell.Occupant.Color),
                        cell.Name);
                }
            }
        }

        private void PlaceTraditionalPieces(Board2 board)
        {
            board.AddPiece(new Rook2(Color.White, false), "A1");
            board.AddPiece(new Knight2(Color.White,false), "B1");
            board.AddPiece(new Bishop2(Color.White, false), "C1");
            board.AddPiece(new Queen2(Color.White, false), "D1");
            board.AddPiece(new King2(Color.White, false), "E1");
            board.AddPiece(new Bishop2(Color.White, false), "F1");
            board.AddPiece(new Knight2(Color.White, false), "G1");
            board.AddPiece(new Rook2(Color.White, false), "H1");

            board.AddPiece(new Pawn2(Color.White, false), "A2");
            board.AddPiece(new Pawn2(Color.White, false), "B2");
            board.AddPiece(new Pawn2(Color.White, false), "C2");
            board.AddPiece(new Pawn2(Color.White, false), "D2");
            board.AddPiece(new Pawn2(Color.White, false), "E2");
            board.AddPiece(new Pawn2(Color.White, false), "F2");
            board.AddPiece(new Pawn2(Color.White, false), "G2");
            board.AddPiece(new Pawn2(Color.White, false), "H2");

            board.AddPiece(new Rook2(Color.Black, false), "A8");
            board.AddPiece(new Knight2(Color.Black, false), "B8");
            board.AddPiece(new Bishop2(Color.Black, false), "C8");
            board.AddPiece(new Queen2(Color.Black, false), "D8");
            board.AddPiece(new King2(Color.Black, false), "E8");
            board.AddPiece(new Bishop2(Color.Black, false), "F8");
            board.AddPiece(new Knight2(Color.Black, false), "G8");
            board.AddPiece(new Rook2(Color.Black, false), "H8");

            board.AddPiece(new Pawn2(Color.Black, false), "A7");
            board.AddPiece(new Pawn2(Color.Black, false), "B7");
            board.AddPiece(new Pawn2(Color.Black, false), "C7");
            board.AddPiece(new Pawn2(Color.Black, false), "D7");
            board.AddPiece(new Pawn2(Color.Black, false), "E7");
            board.AddPiece(new Pawn2(Color.Black, false), "F7");
            board.AddPiece(new Pawn2(Color.Black, false), "G7");
            board.AddPiece(new Pawn2(Color.Black, false), "H7");
        }

        public List<Move2> MoveHistory { get; } = new List<Move2>();

        public int Serial()
        {
            return MoveHistory.Count;
        }

        public TimeSpan TimeSinceLastMove()
        {
            if (MoveHistory.Count == 0)
            {
                return DateTimeOffset.Now - DateTimeOffset.Now;
            }

            return DateTimeOffset.UtcNow - MoveHistory.Last().MoveRecorded;
        }

        public Move2 Move(string from, string to)
        {
            var fromCell = Board.GetCell(from);
            if (fromCell == null)
            {
                throw new ArgumentException("No cell found at location");
            }

            var piece = fromCell.Occupant;
            if (piece == null)
            {
                return Move2.InvalidPlay(null, fromCell, "No piece found at location");
            }

            if (piece.Color != CurrentTurn)
            {
                return Move2.InvalidPlay(null, fromCell, $"It is not {piece.Color}s Turn");
            }

            var toCell = Board.GetCell(to);

            if (toCell == null)
            {
                return Move2.InvalidPlay(piece, fromCell, "Destination cell does not exist");
            }

            var targetPiece = toCell.Occupant;

            if (targetPiece != null)
            {

                if (targetPiece.Color == piece.Color)
                {
                    return Move2.InvalidPlay(piece, fromCell, $"The destination already has a {targetPiece.Color} piece");
                }
            }

            if (piece.IsLegalMoveTo(toCell))
            {
                var outcome = piece.MoveTo(toCell);
            }
            else
            {
                return Move2.InvalidPlay(piece, fromCell, "This destination is not valid for this piece");
            }

            var associatedMoves = new List<Move2>();// POPULATE ON CASTLING
            bool isCheck = false;
            bool isCheckMate = false;

            //TODO: IMPLEMENT LOGIC
            CurrentTurn = CurrentTurn.GetOtherColor();

            var move = Move2.ValidPlay(piece, fromCell, toCell, Serial(), TimeSinceLastMove(), isCheck, isCheckMate, associatedMoves);
            MoveHistory.Add(move);
            return move;
        }

        public Board2 Board { get; }

        public Game2 Clone()
        {
            return new Game2(Style, Board, CurrentTurn);
        }

        public string[] GetPossibleMoves(string location)
        {
            var cell = Board.GetCell(location);
            if (cell.IsOccupied())
            {

                var moves = cell.Occupant.PossibleMoves().Select(x => x.Name).ToArray();
                return moves;
            }
            else
            {
                return new string[0];
            }
        }
    }
}