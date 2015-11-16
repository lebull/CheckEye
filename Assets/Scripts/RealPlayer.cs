using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;
using System;

public class RealPlayer : GamePlayer
{
    [SerializeField] private Aimer aimer;
    private BoardPiece heldBoardPiece;

    void Update()
    {
        aimer.enabled = (gameManager.currentTurnPlayer == this);
    }

    public override void beginTurn(List<BoardMove> validMoves)
    {
        Debug.Log(string.Format("Player's turn began. {0} possible moves.", validMoves.Count));
    }

    //TODO: If gamemanager.turn = me, enable aimer.  Else, disable aimer.

    public void playerClickedSquare(BoardSquare clickedSquare)
    {

        //Highlight valid moves
        if (!heldBoardPiece)
        {
            heldBoardPiece = clickedSquare.gamePiece;
            List<BoardMove> validMoves = gameManager.gameRules.getValidMoveForPiece(board, heldBoardPiece);

            if (validMoves.Count > 0)
            {
                //heldBoardPiecePosition = boardSquare.positionOnBoard;
                heldBoardPiece = clickedSquare.gamePiece;

                //Show valid moves
                foreach (BoardMove validMove in validMoves)
                {
                    board.getSquare(validMove.destination).addHighlight("GAMEMANAGER_VALID_MOVE", new Color(0, 0.5f, 0));
                }
            }
            else
            {
                heldBoardPiece = null;
                //TODO: Play some annoying sound
            }
        }
        else //Player is currently holding a piece and is trying to make a move.
        {
            //Cancle the move
            if (clickedSquare.boardPosition == heldBoardPiece.boardPosition)
            {
                gameManager.board.clearHighlights("GAMEMANAGER_VALID_MOVE");
                heldBoardPiece = null;
            }


            bool validMove = false;

            // Check if the clicked square is a valid move.
            // Because I'm too lazy to figure out how to filter this with a predicate.
            // Probably a good thing overall, anyway.
            foreach(BoardMove targetMove in gameManager.gameRules.getValidMoveForPiece(board, heldBoardPiece))
            {
                if ((clickedSquare.boardPosition.vertical == targetMove.destination.vertical)
                    && (clickedSquare.boardPosition.horizontal == targetMove.destination.horizontal))
                {
                    validMove = true;
                    playerPickedMove(targetMove);
                    break;
                }
            }

            if(validMove)
            {
                gameManager.board.clearHighlights("GAMEMANAGER_VALID_MOVE");
                Debug.Log("Player made a move! Time to call the game manager.");
                //gameManager.playerChoseValidSquare(heldBoardPiece, clickedSquare.boardPosition);
                heldBoardPiece = null;
            }
            else
            {
                //Play an annoying sound
            }
        }
    }
}
