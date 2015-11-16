using UnityEngine;
using System.Collections;
using CheckEye.Board;
using System;
using System.Collections.Generic;




public class RandoAIPlayer : GamePlayer
{
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
        yield return new WaitForSeconds(1f);
        playerPickedMove(validMoves[UnityEngine.Random.Range(0, validMoves.Count)]);
    }

}
