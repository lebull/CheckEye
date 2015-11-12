using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Utility;

public class LocalPlayer : MonoBehaviour
{

    //private BoardPosition heldGamePiecePosition;
    private BoardPiece heldGamePiece;
    private bool holdingGamePiece;

    private GameManager gameManager;
    private Board board;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
    }

    public void playerClickedSquare(BoardSquare boardSquare)
    {
        //Will be a state machine between piece held and no piece held.
        //For now, lets just highlight valid moves.
        Debug.Log(boardSquare.boardPosition);

        if (!holdingGamePiece)
        {
            heldGamePiece = boardSquare.gamePiece;
            List<BoardPosition> validMoves = gameManager.getValidMoves(heldGamePiece);

            if (validMoves.Count > 0)
            {
                holdingGamePiece = true;
                //heldGamePiecePosition = boardSquare.positionOnBoard;
                heldGamePiece = boardSquare.gamePiece;

                //Show valid moves
                foreach (BoardPosition validMove in validMoves)
                {
                    board.getSquare(validMove).addHighlight("GAMEMANAGER_VALID_MOVE", new Color(0, 0.5f, 0));
                }
            }
            else
            {
                heldGamePiece = null;
                //TODO: Play some annoying sound
            }
        }
        else //Player is currently holding a piece and is trying to make a move.
        {
            //Cancle the move
            if (boardSquare.boardPosition == heldGamePiece.boardPosition)
            {
                holdingGamePiece = false;
                gameManager.board.clearHighlights("GAMEMANAGER_VALID_MOVE");
            }
            //WOW!  TODO: This one is a doozer
            if (gameManager.getValidMoves(heldGamePiece).Contains(boardSquare.boardPosition))
            {
                holdingGamePiece = false;
                gameManager.board.clearHighlights("GAMEMANAGER_VALID_MOVE");
                Debug.Log("Player made a move! Time to call the game manager.");
            }
            else
            {
                //Play an annoying sound
            }
        }
    }

    //Events we can add:
    //Player Selects Move
    //Player Defers

    //Events we need from others
    // Do something when its my turn
}
