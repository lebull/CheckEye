using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CheckEye.Board {


    public class BoardSquare : MonoBehaviour {


        public BoardPosition boardPosition{ get { return _boardPosition; }}
        private BoardPosition _boardPosition;

        public bool occupied { get { return gamePiece != null; } }

        public BoardPiece gamePiece;

        //Highlighting

        public bool focused;

        private Aimer aimer;
        private GameObject squareHighlighter;
        public Dictionary<string, Color> activeHighlights;
        public const float highlightAlpha = 0.35f;


        private const string CURSOR_HIGHLIGHT_KEY = "SELF_CURSOR";

        // Use this for initialization
        void Start () {

            focused = false;

            activeHighlights = new Dictionary<string, Color>();

            squareHighlighter = Instantiate((GameObject)Resources.Load("Prefabs/squareCursor"));
            squareHighlighter.transform.position = transform.position + new Vector3(0, 0.01f, 0);
            squareHighlighter.transform.localScale = transform.lossyScale;
            squareHighlighter.transform.parent = transform;
            squareHighlighter.SetActive(false);
        }

        /// <summary>
        /// Adds a highlight to this square.  Highlight output color is the average of all active highlights.
        /// </summary>
        /// <param name="highlightKey"></param>
        /// <param name="highlightColor"></param>
        public void addHighlight(string highlightKey, Color highlightColor)
        {
            activeHighlights[highlightKey] = highlightColor;
            resetHighlightColor();
        }

        /// <summary>
        /// Remove this square's highlight with a particular key
        /// </summary>
        /// <param name="highlightKey"></param>
        public void removeHighlight(string highlightKey)
        {
            activeHighlights.Remove(highlightKey);
            resetHighlightColor();
        }

        void resetHighlightColor()
        {
            if(activeHighlights.Count > 0) {
                squareHighlighter.SetActive(true);
                //Find the average color of the highlights.
                Color averageColor = Color.black;
                foreach (Color color in activeHighlights.Values)
                {
                    averageColor += color;
                }
                averageColor /= activeHighlights.Count;
                averageColor.a = highlightAlpha;
                squareHighlighter.GetComponent<Renderer>().material.color = averageColor;
            }
            else
            {
                squareHighlighter.SetActive(false);
            }

        }

        /// <summary>
        /// Sets the boardsquare 'focused' highlight.
        /// </summary>
        /// <param name="focused_in">True = on</param>
        public void setFocused(bool focused_in)
        {
            focused = focused_in;

            if (focused)
            {
                addHighlight(CURSOR_HIGHLIGHT_KEY, new Color(0.86f, 0.87f, 0.22f));
            }
            if (!focused)
            {
                removeHighlight(CURSOR_HIGHLIGHT_KEY);
            }
        }


        //TODO: This should move the piece if it already has a nonequivilant boardPosition
        public void setBoardPosition(BoardPosition pos)
        {
            _boardPosition = pos;
            string newName = string.Format("BoardSquare ({0}, {1})", pos.horizontal, pos.vertical);
            transform.name = newName;
        }


        /// <summary>
        /// Adds a gamePiece to this board.
        /// </summary>
        /// <param name="gamePiece"></param>
        public void addGamePiece(BoardPiece gamePiece)
        {
            gamePiece.boardSquare = this;
        }
    }
}