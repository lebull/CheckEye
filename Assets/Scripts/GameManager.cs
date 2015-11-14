using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;


public class GameManager : MonoBehaviour {

    public Board board;

    private GameRules _gameRules;
	public GameRules gameRules { get { return _gameRules; } }

    // Use this for initialization
    void Start () {

        LocalPlayer.playerID = GameRules.Player.player_one;

        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();

		//This can be replaced by another game.
		_gameRules = new GameRules();
        gameRules.setUpBoard(board);

    }

    /// <summary>
    /// LocalPlayer calls this when they make a valid move.  Will move the piece on the board.
    /// It should change turns and do shit when it's not your turn or its not a valid move.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="destination"></param>
    public void playerChoseValidSquare(BoardPiece piece, BoardPosition destination)
    {
        //LocalPlayer.playerID;
        //Todo: Execute Move
        //BoardMove move = ..
        piece.move(board.getSquare(destination));
    }

    /// <summary>
    /// Execute a board move, moving the piece, removing targeted pieces, and transforming the gamepiece.
    /// </summary>
    /// <param name="move"></param>
    public void executeMove(BoardMove move)
    {
        //LocalPlayer.playerID;
        //Todo: Execute Move
        //BoardMove move = ..
        move.movingPiece.move(board.getSquare(move.destination));

        //Byebye
        foreach(BoardPiece byebyePiece in move.destroyedPieces)
        {
            Destroy(byebyePiece.gameObject);
        }

    }



    //Todo: LocalPlayerMakes Move
    //  Let other playr know it's their turn.





}
