using UnityEngine;
using System.Collections;
using CheckEye.Utility;

public class Board : MonoBehaviour
{

    public static int GRIDSIZE = 8;

    public bool highlightBoard;

    private float boardPadding = 0.95f;
    private GameObject[,] squareGrid;
    
    public float squarewidth { get{ return (transform.localScale.x * boardPadding) / GRIDSIZE; } }
    

    // Use this for initialization
    void Start()
    {

		spawnBoardSquares();

        
    }

	private void spawnBoardSquares(){
		//Spawn boardSquares.
		GameObject blackSquarePrefab = Resources.Load<GameObject>("Prefabs/BlackSquare");
		GameObject whiteSquarePrefab = Resources.Load<GameObject>("Prefabs/WhiteSquare");
		
		squareGrid = new GameObject[GRIDSIZE, GRIDSIZE];
		
		//Spawn our grid
		float separation = squarewidth;
		Vector3 squareScale = new Vector3(
			separation,
			blackSquarePrefab.transform.localScale.y,
			separation
			);
		
		//1 to 8
		for (int vertical_index = 0; vertical_index < GRIDSIZE; vertical_index++)
		{
			//a to h
			for (int horizontal_index = 0; horizontal_index < GRIDSIZE; horizontal_index++)
			{
				//squareToSpawn is black or white
				GameObject squareToSpawn;
				if ((vertical_index + horizontal_index) % 2 == 0)
					squareToSpawn = blackSquarePrefab;
				else
					squareToSpawn = whiteSquarePrefab;
				
				//This should be local to this.transform
				Vector3 spawnPosition = new Vector3(
					(horizontal_index - (GRIDSIZE / 2)) * separation + separation / 2,
					0,
					(vertical_index - (GRIDSIZE / 2)) * separation + separation / 2
					);
				
				GameObject newSquare = (GameObject)Instantiate(squareToSpawn, spawnPosition, transform.rotation);
				squareGrid[horizontal_index, vertical_index] = newSquare;
				newSquare.transform.localScale = squareScale;
				newSquare.transform.parent = transform;
				newSquare.GetComponent<BoardSquare>().setBoardIndex(horizontal_index, vertical_index);
			}
		}
	}

    /// <summary>
    /// Spawn a piece at this square.  The gamePiecePrefab MUST have a BoardPiece script attatched.
    /// </summary>
    /// <param name="horizontal_index"></param>
    /// <param name="vertical_index"></param>
    /// <param name="gamePiecePrefab"></param>
    /// <returns></returns>
    public GameObject spawnGamePiecePrefabAtPosition(BoardPosition boardPosition, GameObject gamePiecePrefab)
    {
        if(gamePiecePrefab.GetComponent<BoardPiece>() == null)
        {
            throw new System.Exception(string.Format("{} does not have a BoardPiece component.", gamePiecePrefab));
        }

        GameObject spawnSquare = squareGrid[boardPosition.horizontal, boardPosition.vertical];

        GameObject newPiece = (GameObject)Instantiate(
                    gamePiecePrefab,
                    spawnSquare.transform.position + new Vector3(0, spawnSquare.transform.lossyScale.y / 2 + gamePiecePrefab.transform.lossyScale.y / 2, 0),
                    spawnSquare.transform.rotation);
        newPiece.transform.parent = spawnSquare.transform;
        newPiece.transform.localScale = gamePiecePrefab.transform.localScale * transform.localScale.x;

        spawnSquare.GetComponent<BoardSquare>().addGamePiece(newPiece.GetComponent<BoardPiece>());

        return newPiece;
    }

    //TODO: GetPieceAtSquare
    //TODO: MovePiece
    /// <summary>
    /// Clears all highlights with a given key.
    /// </summary>
    /// <param name="highlightKey"></param>
    public void clearHighlights(string highlightKey)
    {
        for(int h_index = 0; h_index < GRIDSIZE; h_index++)
        {
            for (int v_index = 0; v_index < GRIDSIZE; v_index++)
            {
                getSquare(new BoardPosition(h_index, v_index)).removeHighlight(highlightKey);
            }
        }
    }

    /// <summary>
    /// Returns true if the square has a gamepiece.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool isPositionIsOccupied(BoardPosition pos)
    {
        if (!pos.inBoard)
        {
            return false;
        }
        return getSquare(pos).occupied;
    }

    /// <summary>
    /// Returns the BoardSquare at this position.  Returns null if the position isn't on the board.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public BoardSquare getSquare(BoardPosition pos)
    {
        if(!pos.inBoard)
        {
            return null;
        }
        return squareGrid[pos.horizontal, pos.vertical].GetComponent<BoardSquare>();
    }
}