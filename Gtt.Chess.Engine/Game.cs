using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Gtt.Chess.Engine.Pieces;

[assembly: InternalsVisibleTo("Gtt.Chess.Tests")]

namespace Gtt.Chess.Engine
{
    public class Game
    {
        public GameStyle Style { get; }
        public DateTimeOffset? StartTime { get; protected set; }
        public DateTimeOffset? EndTime { get; protected set; }

        internal Board Board { get; }

        public Game(GameStyle style) : this(style, new string[0])
        {

        }

        public Game(GameStyle style, string[] history) : this(style, history, "", "")
        {

        }

        internal Game(GameStyle style, string[] history, string nextFrom, string nextTo)
        {
            Style = style;
            Board = new Board();

            switch (style)
            {
                case GameStyle.Traditional:

                    new Rook(Board, Color.White, Board.GetCell("A1"));
                    new Knight(Board, Color.White, Board.GetCell("B1"));
                    new Bishop(Board, Color.White, Board.GetCell("C1"));
                    new Queen(Board, Color.White, Board.GetCell("D1"));
                    new King(Board, Color.White, Board.GetCell("E1"));
                    new Bishop(Board, Color.White, Board.GetCell("F1"));
                    new Knight(Board, Color.White, Board.GetCell("G1"));
                    new Rook(Board, Color.White, Board.GetCell("H1"));

                    new Pawn(Board, Color.White, Board.GetCell("A2"));
                    new Pawn(Board, Color.White, Board.GetCell("B2"));
                    new Pawn(Board, Color.White, Board.GetCell("C2"));
                    new Pawn(Board, Color.White, Board.GetCell("D2"));
                    new Pawn(Board, Color.White, Board.GetCell("E2"));
                    new Pawn(Board, Color.White, Board.GetCell("F2"));
                    new Pawn(Board, Color.White, Board.GetCell("G2"));
                    new Pawn(Board, Color.White, Board.GetCell("H2"));

                    new Rook(Board, Color.Black, Board.GetCell("A8"));
                    new Knight(Board, Color.Black, Board.GetCell("B8"));
                    new Bishop(Board, Color.Black, Board.GetCell("C8"));
                    new Queen(Board, Color.Black, Board.GetCell("D8"));
                    new King(Board, Color.Black, Board.GetCell("E8"));
                    new Bishop(Board, Color.Black, Board.GetCell("F8"));
                    new Knight(Board, Color.Black, Board.GetCell("G8"));
                    new Rook(Board, Color.Black, Board.GetCell("H8"));

                    new Pawn(Board, Color.Black, Board.GetCell("A7"));
                    new Pawn(Board, Color.Black, Board.GetCell("B7"));
                    new Pawn(Board, Color.Black, Board.GetCell("C7"));
                    new Pawn(Board, Color.Black, Board.GetCell("D7"));
                    new Pawn(Board, Color.Black, Board.GetCell("E7"));
                    new Pawn(Board, Color.Black, Board.GetCell("F7"));
                    new Pawn(Board, Color.Black, Board.GetCell("G7"));
                    new Pawn(Board, Color.Black, Board.GetCell("H7"));

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style));
            }

            if (history != null && history.Length > 0 ||(!string.IsNullOrWhiteSpace(nextFrom) && !string.IsNullOrWhiteSpace(nextTo)))
            {
                Board.ReplayHistory(history, nextFrom, nextTo);
            }
        }

        public void Start()
        {
            StartTime = DateTimeOffset.UtcNow;
        }

        public string[] History()
        {
            return Board.History;
        }

        public string[] GetPossibleMoves(string location)
        {
            var cell = Board.GetCell(location);
            if (cell.CurrentPiece == null)
            {
                return new String[0];
            }

            var moves = cell.CurrentPiece.PossibleMoves().Select(x => x.Name).ToArray();
            return moves;
        }

        public GameMoveResult Move(string from, string to)
        {
            if (StartTime == null)
            {
                Start();
            }

            var result = Board.Move(from, to);

            return new GameMoveResult(
                result.Select(x => new PieceMoveResultStatus(
                    x.Piece.Name,
                    x.Piece.Color.ToString(),
                    x.StartingCell.Name,
                    x.EndingCell.Name,
                    x.PieceWon?.Name,
                    x.PieceWon?.Color.ToString())).ToArray(),
                DateTimeOffset.UtcNow,
                Board.WhatColorIsInCheck(),
                null
                );
        }

        //public BoardStatus CurrentBoard()
        //{

        //    var board = new BoardStatus(
        //        Board.Cells.Select(c =>
        //        new CellStatus(c.Name, c.X, c.Y, c.Color, c.CurrentPiece?.Name, c.CurrentPiece?.Color)
        //    ).ToArray(),
        //        Board.CurrentTurn);
        //    return board;
        //}
    }


    public enum GameStyle
    {
        Traditional
    }
}
