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
        private Aimer aimer;
        private bool focused;
        private GameObject squareHighlighter;
        public Dictionary<string, Color> activeHighlights;
        public const float highlightAlpha = 0.35f;


        private const string CURSOR_HIGHLIGHT_KEY = "SELF_CURSOR";

        // Use this for initialization
        void Start () {

            focused = false;

            aimer = GameObject.FindGameObjectWithTag("Aimer").GetComponent<Aimer>();
            activeHighlights = new Dictionary<string, Color>();

            squareHighlighter = Instantiate((GameObject)Resources.Load("Prefabs/squareCursor"));
            squareHighlighter.transform.position = transform.position + new Vector3(0, 0.01f, 0);
            squareHighlighter.transform.localScale = transform.lossyScale;
            squareHighlighter.transform.parent = transform;
            squareHighlighter.SetActive(false);
        }

        // Update is called once per frame
        void Update() {
            if ((!focused) && (aimer.getAimSquare() != null) && (aimer.getAimSquare() == gameObject))
            {
                focused = true;
                addHighlight(CURSOR_HIGHLIGHT_KEY, new Color(0.86f, 0.87f, 0.22f));
            }

            if (focused && (aimer.getAimSquare() != gameObject))
            {
                focused = false;
                removeHighlight(CURSOR_HIGHLIGHT_KEY);
            }

            if (focused && Input.GetMouseButtonDown(0))
            {
                GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<LocalPlayer>().playerClickedSquare(this);
            }
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
        /// DEPRICATE THIS POS NOW.  Use setBoardPosition plz.
        /// </summary>
        /// <param name="horizontal_index"></param>
        /// <param name="vertical_index"></param>
        public void setBoardIndex(int horizontal_index, int vertical_index)
        {
            _boardPosition = new BoardPosition(horizontal_index, vertical_index);
        }

        //TODO: This should move the piece if it already has a nonequivilant boardPosition
        public void setBoardPosition(BoardPosition pos)
        {
            _boardPosition = pos;
        }


        /// <summary>
        /// Adds a gamePiece to this board.
        /// </summary>
        /// <param name="gamePiece"></param>
        public void addGamePiece(BoardPiece gamePiece)
        {
            //_gamePiece = gamePiece;
            gamePiece.boardSquare = this;
        }
    }
}