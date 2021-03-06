﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gtt.Chess.Engine.Extensions;
using Gtt.Chess.Engine.Pieces;

namespace Gtt.Chess.Engine
{
    public class Game
    {
        public GameStyle Style { get; }
        public Guid Id { get; }
        public Color CurrentTurn { get; protected set; }
        public Dictionary<Color, ICollection<Piece>> Captures { get; }

        protected Game()
        {
            Id = Guid.NewGuid();
            Captures = new Dictionary<Color, ICollection<Piece>>
            {
                [Color.White] = new List<Piece>(),
                [Color.Black] = new List<Piece>()
            };

        }

        public Game(GameStyle style) : this()
        {
            Style = style;
            switch (style)
            {
                case GameStyle.Traditional:
                    Board = new Board(this);
                    PlaceTraditionalPieces(Board);
                    CurrentTurn = Color.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        protected Game(GameStyle style, Board copyFrom, Color nextTurn) : this()
        {
            Style = style;
            switch (style)
            {
                case GameStyle.Traditional:
                    Board = new Board(this);
                    CopyBoard(copyFrom);
                    CurrentTurn = nextTurn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        private void CopyBoard(Board copyFrom)
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

        private void PlaceTraditionalPieces(Board board)
        {
            board.AddPiece(new Rook(Color.White, false), "A1");
            board.AddPiece(new Knight(Color.White,false), "B1");
            board.AddPiece(new Bishop(Color.White, false), "C1");
            board.AddPiece(new Queen(Color.White, false), "D1");
            board.AddPiece(new King(Color.White, false), "E1");
            board.AddPiece(new Bishop(Color.White, false), "F1");
            board.AddPiece(new Knight(Color.White, false), "G1");
            board.AddPiece(new Rook(Color.White, false), "H1");

            board.AddPiece(new Pawn(Color.White, false), "A2");
            board.AddPiece(new Pawn(Color.White, false), "B2");
            board.AddPiece(new Pawn(Color.White, false), "C2");
            board.AddPiece(new Pawn(Color.White, false), "D2");
            board.AddPiece(new Pawn(Color.White, false), "E2");
            board.AddPiece(new Pawn(Color.White, false), "F2");
            board.AddPiece(new Pawn(Color.White, false), "G2");
            board.AddPiece(new Pawn(Color.White, false), "H2");

            board.AddPiece(new Rook(Color.Black, false), "A8");
            board.AddPiece(new Knight(Color.Black, false), "B8");
            board.AddPiece(new Bishop(Color.Black, false), "C8");
            board.AddPiece(new Queen(Color.Black, false), "D8");
            board.AddPiece(new King(Color.Black, false), "E8");
            board.AddPiece(new Bishop(Color.Black, false), "F8");
            board.AddPiece(new Knight(Color.Black, false), "G8");
            board.AddPiece(new Rook(Color.Black, false), "H8");

            board.AddPiece(new Pawn(Color.Black, false), "A7");
            board.AddPiece(new Pawn(Color.Black, false), "B7");
            board.AddPiece(new Pawn(Color.Black, false), "C7");
            board.AddPiece(new Pawn(Color.Black, false), "D7");
            board.AddPiece(new Pawn(Color.Black, false), "E7");
            board.AddPiece(new Pawn(Color.Black, false), "F7");
            board.AddPiece(new Pawn(Color.Black, false), "G7");
            board.AddPiece(new Pawn(Color.Black, false), "H7");
        }

        public List<Move> MoveHistory { get; } = new List<Move>();

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

        public Move Move(string from, string to)
        {
            var fromCell = Board.GetCell(from);
            if (fromCell == null)
            {
                throw new ArgumentException("No cell found at location");
            }

            var piece = fromCell.Occupant;
            if (piece == null)
            {
                return Engine.Move.InvalidPlay(null, fromCell, "No piece found at location");
            }

            if (piece.Color != CurrentTurn)
            {
                return Engine.Move.InvalidPlay(null, fromCell, $"It is not {piece.Color}s Turn");
            }

            var toCell = Board.GetCell(to);

            if (toCell == null)
            {
                return Engine.Move.InvalidPlay(piece, fromCell, "Destination cell does not exist");
            }

            var targetPiece = toCell.Occupant;

            if (targetPiece != null)
            {

                if (targetPiece.Color == piece.Color)
                {
                    return Engine.Move.InvalidPlay(piece, fromCell, $"The destination already has a {targetPiece.Color} piece");
                }
            }

            bool isCheck = false;
            bool isCheckMate = false;

            if (piece.IsLegalMoveTo(toCell))
            {
                var simulatedMove = SimulateMove(from, to);
                var inCheck = simulatedMove.PiecesInCheck();
                if (inCheck.Any(x => x.Color == CurrentTurn))
                {
                    return Engine.Move.InvalidPlay(piece, fromCell, $"Cannot make this move. This move would put ${piece.Color} in check.");
                }

                if (inCheck.Any())
                {
                    isCheck = true;
                    var inCheckMate = simulatedMove.PiecesInCheckMate();
                    if (inCheckMate.Any(x => x.Color == CurrentTurn))
                    {
                        return Engine.Move.InvalidPlay(piece, fromCell, $"Cannot make this move. This move would put ${piece.Color} in checkmate.");
                    }
                    isCheckMate = true;
                }

                var outcome = piece.MoveTo(toCell);
            }
            else
            {
                return Engine.Move.InvalidPlay(piece, fromCell, "This destination is not valid for this piece");
            }

            var associatedMoves = new List<Move>();// POPULATE ON CASTLING

            //TODO: IMPLEMENT LOGIC
            CurrentTurn = CurrentTurn.GetOtherColor();

            var move = Engine.Move.ValidPlay(piece, fromCell, toCell, Serial(), TimeSinceLastMove(), isCheck, isCheckMate, associatedMoves);
            MoveHistory.Add(move);
            return move;
        }

        private King[] PiecesInCheck()
        {
            var kings = Board.GetPieces<King>();
            King[] r = kings.Where(x => x.CanBeTakenBy().Any()).ToArray();
            return r;
        }

        private King[] PiecesInCheckMate()
        {
            var kings = Board.GetPieces<King>().ToList();
            Dictionary<Color, bool> canBeTaken = new Dictionary<Color, bool>
            {
                {Color.White, false},
                {Color.Black, false}
            };

            // CAN THE ATTACKER BE REMOVED
            

            foreach (var king in kings)
            {
                // CAN THE ATTACKER BE TAKEN
                foreach (var piece in king.CanBeTakenBy())
                {
                    if (piece.CanBeTakenBy().Any())
                    {
                        canBeTaken[piece.Color] = true;
                    }
                }

                // CAN THE KING MOVE OUT OF THE WAY
                if (canBeTaken.Any(x => x.Value == false && x.Key == king.Color))
                {
                    foreach (var move in king.PossibleMoves())
                    {
                        var sim = SimulateMove(king.CurrentCell.Name, move.Name);
                        if (sim.PiecesInCheck().Contains(king))
                        {
                            canBeTaken[king.Color] = true;
                            break;
                        }
                    }
                }

                // CAN BE BLOCKED
                //TODO:
            }

            return kings.Where(x => canBeTaken.Where(y => y.Value).Select(y => y.Key).Contains(x.Color)).ToArray();
        }

        public Board Board { get; }

        public Game Clone()
        {
            return new Game(Style, Board, CurrentTurn);
        }

        public Game SimulateMove(string from, string to)
        {
            var g = Clone();
            var fromCell = g.Board.GetCell(from);
            var toCell = g.Board.GetCell(to);
            fromCell.Occupant.MoveTo(toCell);
            return g;
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