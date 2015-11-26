using UnityEngine;
using System.Collections;
using CheckEye.Board;
using System;
using System.Collections.Generic;

public class RandoAIPlayer : GamePlayer
{
    public float delay = 0.1f;

    public override void beginTurn( List<BoardMove> validMoves)
    {
        //throw new NotImplementedException();
        StartCoroutine(pickRandomMove(validMoves));
    }

    /// <summary>
    /// Pick a random valid move.
    /// </summary>
    IEnumerator pickRandomMove(List<BoardMove> validMoves)
    {
        yield return new WaitForSeconds(delay);

        BoardMove randomMove = validMoves[UnityEngine.Random.Range(0, validMoves.Count)];

        Debug.Log(string.Format("From: {0}\t To:{1}", randomMove.movingPiece.boardSquare.boardPosition, randomMove.destination));

        playerPickedMove(randomMove);
    }

}
