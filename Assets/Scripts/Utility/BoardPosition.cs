using UnityEngine;
using System.Collections;

namespace CheckEye.Utility {
    public class BoardPosition {

        public int vertical;
        public int horizontal;

        public BoardPosition(int horizontal, int vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }

        public bool inBoard
        {
            get { return (horizontal    >= 0 
                        && vertical     >= 0 
                        && horizontal   < Board.GRIDSIZE 
                        && vertical     < Board.GRIDSIZE);
            }
        }

        public static BoardPosition operator +(BoardPosition first, BoardPosition second)
        {


            return new BoardPosition(first.horizontal + second.horizontal, first.vertical + second.vertical);

        }
    }
}