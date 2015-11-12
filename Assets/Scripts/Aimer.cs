using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {

    private int BoardSquareLayer;
    private LayerMask boardSquareLayerMask;

	// Use this for initialization
	void Start () {
        BoardSquareLayer = LayerMask.NameToLayer("BoardSquare");
        boardSquareLayerMask = ~BoardSquareLayer;

    }
	
	// Update is called once per frame
	void Update () {
        getAimSquare();
    }

    public GameObject getAimSquare()
    {
        //http://answers.unity3d.com/questions/252847/check-if-raycast-hits-layer.html

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, boardSquareLayerMask) && hit.transform.gameObject.layer == BoardSquareLayer)
            return hit.transform.gameObject;

        return null;
        
    }
}
