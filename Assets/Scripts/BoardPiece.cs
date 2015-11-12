using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CheckEye.Utility;


public class BoardPiece : MonoBehaviour {

    public BoardSquare boardSquare;
    public BoardPosition boardPosition { get { return boardSquare.boardPosition; } }
    public GameManager.Player owner;
    public bool selected;

	public void setGamepieceData(BoardSquare parentBoardSquare, GameManager.Player owner){
		boardSquare = parentBoardSquare;
		this.owner = owner;
	}

	void onSelect(){
		//Highlight squares of available moves
	}

	void onUnSelect(){
		//Unhighlight squares of available moves
	}
}
