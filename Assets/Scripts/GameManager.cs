using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;


public class GameManager : MonoBehaviour {

    public Board board;

    private List<GamePlayer> players;
    public GamePlayer currentTurnPlayer;

    private GameRules _gameRules;
	public GameRules gameRules { get { return _gameRules; } }

    // Use this for initialization
    void Start () {

        RealPlayer localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<RealPlayer>();

        if (localPlayer == null)
        {
            throw new System.Exception("Object not found");
        }

        players = new List<GamePlayer>();
        players.Add(localPlayer);
        GameObject playerTwoObject = new GameObject();
        RandoAIPlayer playerTwo = playerTwoObject.AddComponent<RandoAIPlayer>();
        playerTwo.setInitialProperties(board, this);
        //ISSUE:  RandoAIPlayer is not getting its initial values assigned to it.  Need a prefab.
        players.Add(playerTwoObject.GetComponent<RandoAIPlayer>());

        currentTurnPlayer = players[0];

        board = GameObject.Find("Board").GetComponent<Board>();

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

        //TODO: Transform

        //Next turn.  TODO: This should go away when it's implemented into the move.
        nextPlayerTurn();

    }

    /// <summary>
    /// Passes the current 'playing' player to the next in line, ie. at the end of a move.
    /// Also, calls startTurn on the new player.
    /// </summary>
    public void nextPlayerTurn()
    {
        int nextTurnIndex = (players.IndexOf(currentTurnPlayer) + 1) % players.Count;
        currentTurnPlayer = players[nextTurnIndex];
        currentTurnPlayer.beginTurn(gameRules.getValidMoves(board));  //ISSUE
    }


    //Todo: LocalPlayerMakes Move
    //  Let other playr know it's their turn.
}
