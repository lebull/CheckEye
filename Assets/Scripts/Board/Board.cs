using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CheckEye.Board {

    public class Board : MonoBehaviour
    {

        public static int GRIDSIZE = 8;   //How many squares are along the edge of the grid
        public const int PLANESCALE = 10; //How large an edge of our rendered plane is

        public float squarewidth { get{ return (transform.localScale.x * PLANESCALE) / GRIDSIZE; } }

        private GameObject[,] squareGrid;

        // Use this for initialization
        void Start()
        {
		    spawnBoardSquares();
        }

	    private void spawnBoardSquares(){
            //Spawn boardSquares.
            //GameObject blackSquarePrefab = Resources.Load<GameObject>("Prefabs/BlackSquare");
            //GameObject whiteSquarePrefab = Resources.Load<GameObject>("Prefabs/WhiteSquare");

            GameObject newSquarePrefab = new GameObject();
            newSquarePrefab.transform.localScale = new Vector3(1 / GRIDSIZE * PLANESCALE, 0.5f, 1 / GRIDSIZE * PLANESCALE);
            newSquarePrefab.AddComponent<BoardSquare>();
            newSquarePrefab.name = "BoardSquare";
            newSquarePrefab.AddComponent<BoxCollider>();
            newSquarePrefab.layer = LayerMask.NameToLayer("BoardSquare");

            squareGrid = new GameObject[GRIDSIZE, GRIDSIZE];
		
		    //Spawn our grid
		    float separation = squarewidth;
		    Vector3 squareScale = new Vector3(
			    separation,
                newSquarePrefab.transform.localScale.y * transform.localScale.y,
			    separation
			    );
		
		    //1 to 8
		    for (int vertical_index = 0; vertical_index < GRIDSIZE; vertical_index++)
		    {
			    //a to h
			    for (int horizontal_index = 0; horizontal_index < GRIDSIZE; horizontal_index++)
			    {				
				    //This should be local to this.transform
				    Vector3 spawnPosition = new Vector3(
					    (horizontal_index - (GRIDSIZE / 2)) * separation + separation / 2,
					    0,
					    (vertical_index - (GRIDSIZE / 2)) * separation + separation / 2
					    );
				
				    GameObject newSquare = (GameObject)Instantiate(newSquarePrefab, transform.position + spawnPosition, transform.rotation);
				    squareGrid[horizontal_index, vertical_index] = newSquare;
                    BoardPosition newBoardPosition = new BoardPosition(horizontal_index, vertical_index);
				    newSquare.transform.localScale = squareScale;
				    newSquare.transform.parent = transform;
				    newSquare.GetComponent<BoardSquare>().setBoardPosition(newBoardPosition);
			    }
		    }
	    }

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

        public List<BoardPiece> allPieces()
        {
            List<BoardPiece> returnList = new List<BoardPiece>();
            for (int horz = 0; horz < GRIDSIZE; horz++)
            { 
                for (int vert = 0; vert < GRIDSIZE; vert++)
                {
                    BoardPosition boardPosition = new BoardPosition(horz, vert);
                    if (getSquare(boardPosition).occupied)
                    {
                        returnList.Add(getSquare(boardPosition).gamePiece);
                    }
                    
                }
            }

            return returnList;
        }

        /// <summary>
        /// Clears all highlights of every square.
        /// </summary>
        public void clearAllHighlights()
        {
            foreach (GameObject boardSquare in squareGrid)
            {
                boardSquare.GetComponent<BoardSquare>().clearAllHighlights();
            }
        }

        /// <summary>
        /// Clear the board.
        /// </summary>
        public void wipe()
        {
            foreach (GameObject boardSquare in squareGrid)
            {
                if(boardSquare.GetComponent<BoardSquare>().gamePiece != null)
                {
                    DestroyImmediate(boardSquare.GetComponent<BoardSquare>().gamePiece.gameObject);
                }
            }
        }
    }
}