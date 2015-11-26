using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CheckEye.Board { 



    public class BoardPiece : MonoBehaviour {

        public Board board;
        public BoardSquare boardSquare;
        public BoardPosition boardPosition { get { return boardSquare.boardPosition; } }
        public GamePlayer owner;

        public enum BoardPieceColors
        {
            RED,
            BLACK
        }
        public BoardPieceColors color;

        //Todo: To checker piece subclass
        public bool kinged;

        public static void CreateBoardPiece(Board board, GameObject boardPiecePrefab, BoardSquare boardSquare, GamePlayer owner, BoardPieceColors color)
        {
            if (boardPiecePrefab.GetComponent<BoardPiece>() == null)
            {
                throw new System.Exception(string.Format("{} does not have a BoardPiece component.", boardPiecePrefab));
            }

            GameObject newPiece = Instantiate(boardPiecePrefab);

            newPiece.transform.parent = boardSquare.transform;
            newPiece.transform.localScale = Vector3.Scale(newPiece.transform.localScale, board.transform.localScale);

            BoardPiece newBoardPiece = newPiece.GetComponent<BoardPiece>();

            newBoardPiece.owner = owner;
            newBoardPiece.board = board;
            newBoardPiece.color = color;

            newBoardPiece.move(boardSquare);
        }

        public void move(BoardSquare destination)
        {
            if (destination.occupied)
            {
                throw new System.Exception("Cannot move a piece to an occupied spot");
            }

            BoardSquare oldSquare = boardSquare;
            BoardSquare newSquare = destination;

            newSquare.gamePiece = this;

            if (oldSquare != null) {
                oldSquare.gamePiece = null;
            }
            else
            {
                transform.localPosition = Vector3.zero + transform.up*transform.localScale.y;
            }

            Vector3 posOffset = transform.localPosition;
            transform.parent = newSquare.transform;
            transform.localPosition = posOffset;

            boardSquare = newSquare;
        
        }

        void OnDestroy()
        {
            boardSquare.gamePiece = null;
        }
    }
}