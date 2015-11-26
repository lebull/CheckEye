using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Board;

public class GameRules
{

    private delegate void addJumpMoveDelegate(int i, int j);

    /// <summary>
    /// Returns a list of valid boardPositions that a piece can move to.
    /// </summary>
    /// <param name="boardPiece"></param>
    /// <returns></returns>
    public List<BoardMove> getValidMoveForPiece(Board board, BoardPiece boardPiece, GamePlayer requestingPlayer)
    {
        if (boardPiece.owner != requestingPlayer)
        {
            return new List<BoardMove>();
        }

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
                    validMoves.Add(new BoardMove(
                                        boardPiece,
                                        farSquare.boardPosition,
                                        new List<BoardPiece>() { destroyedPiece }
                    ));
                }
            };

            if (boardPiece.color == BoardPiece.BoardPieceColors.BLACK || boardPiece.kinged)
            {
                addJumpMove(-1, 1);
                addJumpMove(1, 1);
            }

            if (boardPiece.color == BoardPiece.BoardPieceColors.RED || boardPiece.kinged)
            {
                addJumpMove(-1, -1);
                addJumpMove(1, -1);
            }

            List<BoardPosition> validDestinations = new List<BoardPosition>();
            if (boardPiece.color == BoardPiece.BoardPieceColors.BLACK)
            {
                validDestinations.Add(piecePosition + new BoardPosition(-1, 1));
                validDestinations.Add(piecePosition + new BoardPosition(1, 1));
            }
            if (boardPiece.color == BoardPiece.BoardPieceColors.RED)
            {
                validDestinations.Add(piecePosition + new BoardPosition(-1, -1));
                validDestinations.Add(piecePosition + new BoardPosition(1, -1));
            }

            //Remove invalid moves
            validDestinations.RemoveAll(move => !move.inBoard || board.isPositionIsOccupied(move));

            foreach (BoardPosition moveDestination in validDestinations)
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

    public List<BoardMove> getValidMoves(Board board, GamePlayer player)
    {
        List<BoardMove> returnList = new List<BoardMove>();
        foreach (BoardPiece piece in board.allPieces().FindAll(piece => piece != null))
        {
            returnList.AddRange(getValidMoveForPiece(board, piece, player));
        }
        return returnList;
    }


    /// <summary>
    /// Callback to spawn the board.
    /// </summary>
    /// <param name="board"></param>
    public void setUpBoard(Board board, List<GamePlayer> players)
    {

        if (players.Count != 2)
        {
            throw new System.Exception("'players' parameter must be of length of 2.  Goddamnit, a game of checkers has only 2 players.");
        }

        board.wipe();

        GameObject playerOneGamePiece = (GameObject)Resources.Load("Prefabs/PlayerOneGamePiece");
        GameObject playerTwoGamePiece = (GameObject)Resources.Load("Prefabs/PlayerTwoGamePiece");

        for (int horizontal_index = 0; horizontal_index < Board.GRIDSIZE; horizontal_index++)
        {
            for (int rel_vertical_index = 0; rel_vertical_index < 3; rel_vertical_index++)
            {

                int playerOneVerticalIndex = rel_vertical_index;
                int playerTwoVerticalIndex = Board.GRIDSIZE - 1 - rel_vertical_index;

                //Only spawn every other square
                if ((horizontal_index + playerOneVerticalIndex) % 2 == 0)
                {
                    //new BoardPosition(horizontal_index, playerOneVerticalIndex);

                    BoardSquare playerOneSquare = board.getSquare(new BoardPosition(horizontal_index, playerOneVerticalIndex));
                    BoardPiece.CreateBoardPiece(board, playerOneGamePiece, playerOneSquare, players[0], BoardPiece.BoardPieceColors.BLACK);
                    
                }
                else
                {
                    BoardSquare playerTwoSquare = board.getSquare(new BoardPosition(horizontal_index, playerTwoVerticalIndex));
                    BoardPiece.CreateBoardPiece(board, playerTwoGamePiece, playerTwoSquare, players[1], BoardPiece.BoardPieceColors.RED);
                }
            }
        }
    }

}
