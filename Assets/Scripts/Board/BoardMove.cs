using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class should NOT be responsible for any game logic, only what happens in a move.
/// </summary>
namespace CheckEye.Board {
    public class BoardMove {

        public BoardPiece movingPiece;
        public BoardPosition source { get { return movingPiece.boardPosition;} }
        public BoardPosition destination;
        public List<BoardPiece> destroyedPieces;
        public BoardPiece transformTo;

        public BoardMove(BoardPiece movingPiece, 
                                BoardPosition destination,
                                List<BoardPiece> destroyedPieces = null, 
                                BoardPiece transformTo = null)
        {
            this.movingPiece = movingPiece;
            this.destination = destination;
            this.transformTo = transformTo;


            if (destroyedPieces == null)
            {
                this.destroyedPieces = new List<BoardPiece>();
            }
            else
            {
                this.destroyedPieces = destroyedPieces;
            }
        }

        /*
        public BoardMove(BoardSquare source, BoardSquare destination)
        {
            //this.source = source;
            //this.destination = destination;
        }*/
	}
}
