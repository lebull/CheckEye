using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;

public class GameRules {

    public enum Player
    {
        player_one = 1,
        player_two = 2
    }

    private delegate void addJumpMoveDelegate(int i, int j);

    /// <summary>
    /// Returns a list of valid boardPositions that a piece can move to.
    /// </summary>
    /// <param name="boardPiece"></param>
    /// <returns></returns>
    public List<BoardMove> getValidMoveForPiece(Board board, BoardPiece boardPiece)
	{
        try
        {
            BoardPosition piecePosition = boardPiece.boardPosition;
            List<BoardMove> validMoves = new List<BoardMove>();


            //This delegate will add any possible jump moves to validMoves
            //Had issues with syntax while trying to make this an anon function.
            addJumpMoveDelegate addJumpMove = delegate (int near_horizontal, int near_vertical)
            {

                BoardSquare closeSquare = board.getSquare(piecePosition + new BoardPosition(near_horizontal, near_vertical));
                BoardSquare farSquare = board.getSquare(piecePosition + new BoardPosition(near_horizontal * 2, near_vertical * 2));

                if (closeSquare && farSquare &&
                    closeSquare.occupied &&
                    closeSquare.gamePiece.owner != boardPiece.owner &&
                    !farSquare.occupied
                    )
                {
                    BoardPiece destroyedPiece = closeSquare.gamePiece;
                    validMoves.Add( new BoardMove(
                                        boardPiece,
                                        farSquare.boardPosition,
                                        new List<BoardPiece>() { destroyedPiece }
                    ));
                }
            };
               

            if (boardPiece.owner == Player.player_one || boardPiece.kinged)
            {
                addJumpMove(-1, 1);
                addJumpMove(1, 1);
            }

            if (boardPiece.owner == Player.player_two || boardPiece.kinged)
            {
                addJumpMove(-1, -1);
                addJumpMove(1, -1);
            }



            List<BoardPosition> validDestinations = new List<BoardPosition>();
            if (boardPiece.owner == Player.player_one)
		    {
                validDestinations.Add(piecePosition + new BoardPosition(-1, 1));
			    validDestinations.Add(piecePosition + new BoardPosition(1, 1));
		    }
		    if (boardPiece.owner == Player.player_two)
		    {
			    validDestinations.Add(piecePosition + new BoardPosition(-1, -1));
			    validDestinations.Add(piecePosition + new BoardPosition(1, -1));
		    }
		
		    //Remove invalid moves
		    validDestinations.RemoveAll(move => !move.inBoard || board.isPositionIsOccupied(move));

            foreach(BoardPosition moveDestination in validDestinations)
            {
                validMoves.Add(new BoardMove(boardPiece, moveDestination));
            }
            

            return validMoves;
        }
        catch (System.NullReferenceException)
        {
            return new List<BoardMove>();
        }
    }

	
    /// <summary>
    /// Callback to spawn the board.
    /// </summary>
    /// <param name="board"></param>
	public void setUpBoard(Board board){

		GameObject playerOneGamePiece = (GameObject)Resources.Load("Prefabs/PlayerOneGamePiece");
		GameObject playerTwoGamePiece = (GameObject)Resources.Load("Prefabs/PlayerTwoGamePiece");
		
		for (int horizontal_index = 0; horizontal_index < Board.GRIDSIZE; horizontal_index++)
		{
			for (int rel_vertical_index = 0; rel_vertical_index < 3; rel_vertical_index++)
			{
				int playerOneVerticalIndex = rel_vertical_index;
				int playerTwoVerticalIndex = Board.GRIDSIZE - 1 - rel_vertical_index;
				
				new BoardPosition(horizontal_index, playerOneVerticalIndex);
				
				BoardSquare playerOneSquare = board.getSquare(new BoardPosition(horizontal_index, playerOneVerticalIndex));
                BoardSquare playerTwoSquare = board.getSquare(new BoardPosition(horizontal_index, playerTwoVerticalIndex));

                //board.spawnGamePiece(playerOneNewPiecePos, playerOneGamePiece, Player.player_one);
                //board.spawnGamePiece(playerTwoNewPiecePos, playerTwoGamePiece, Player.player_two);
                BoardPiece.CreateBoardPiece(board, playerOneGamePiece, playerOneSquare, Player.player_one);
                BoardPiece.CreateBoardPiece(board, playerTwoGamePiece, playerTwoSquare, Player.player_two);
            }
		}
	}

}
