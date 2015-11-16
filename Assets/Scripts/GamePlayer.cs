using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;

public abstract class GamePlayer : MonoBehaviour
{
    public static GameRules.Player playerID;

    //If we ever have multiple gameManagers, I guess we could make setters.
    //Actually, hell no.  Make prefabs.  Serialize OP
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected Board board;

    private bool myTurn;

    public void setInitialProperties(Board board, GameManager gameManager)
    {
        this.board = board;
        this.gameManager = gameManager;
    }

    public void playerPickedMove(BoardMove chosenMove)
    {
        gameManager.executeMove(chosenMove);
    }

    public abstract void beginTurn(List<BoardMove> validMoves);
}
