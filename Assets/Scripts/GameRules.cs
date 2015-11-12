using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Utility;

public class GameRules {

	/// <summary>
	/// Returns a list of valid boardPositions that a piece can move to.
	/// </summary>
	/// <param name="gamePiece"></param>
	/// <returns></returns>
	public List<BoardPosition> getValidMoves(Board board, BoardPiece gamePiece)
	{

		BoardPosition piecePosition = gamePiece.boardPosition;
		
		//TODO: Here's where we left off in refactoring the boardPosition variable.  We need to be able to see relative spots. Should be an eays struct method.
		List<BoardPosition> returnList = new List<BoardPosition>();
		
		if (gamePiece.owner == GameManager.Player.playerOne)
		{
			returnList.Add(piecePosition + new BoardPosition(-1, 1));
			returnList.Add(piecePosition + new BoardPosition(1, 1));
		}
		if (gamePiece.owner == GameManager.Player.playerTwo)
		{
			returnList.Add(piecePosition + new BoardPosition(-1, -1));
			returnList.Add(piecePosition + new BoardPosition(1, -1));
		}
		
		//Remove invalid moves
		returnList.RemoveAll(move => !move.inBoard || board.isPositionIsOccupied(move));
		
		return returnList;
	}

	

	public void setUpBoard(Board board){

		GameObject playerOneGamePiece = (GameObject)Resources.Load("Prefabs/PlayerOneGamePiece");
		GameObject playerTwoGamePiece = (GameObject)Resources.Load("Prefabs/PlayerTwoGamePiece");

		//List<GameObject> redPieces = new List<GameObject>();
		//List<GameObject> bluePieces = new List<GameObject>();

		Debug.Log (board);
		
		for (int horizontal_index = 0; horizontal_index < Board.GRIDSIZE; horizontal_index++)
		{
			for (int rel_vertical_index = 0; rel_vertical_index < 3; rel_vertical_index++)
			{
				int playerOneVerticalIndex = rel_vertical_index;
				int playerTwoVerticalIndex = Board.GRIDSIZE - 1 - rel_vertical_index;
				
				new BoardPosition(horizontal_index, playerOneVerticalIndex);
				
				BoardPosition playerOneNewPiecePos = new BoardPosition(horizontal_index, playerOneVerticalIndex);
				BoardPosition playerTwoNewPiecePos = new BoardPosition(horizontal_index, playerTwoVerticalIndex);
				
				board.spawnGamePiecePrefabAtPosition(playerOneNewPiecePos, playerOneGamePiece);
				board.spawnGamePiecePrefabAtPosition(playerTwoNewPiecePos, playerTwoGamePiece);
			}
		}
	}

}
