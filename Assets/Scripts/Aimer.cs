using UnityEngine;
using System.Collections;

using CheckEye.Board;

public class Aimer : MonoBehaviour {

    [SerializeField]
    private GameObject aimingPlayer;
    [SerializeField] private GameObject _aimObject;
    public GameObject aimObject { get { return _aimObject; } }

	// Update is called once per frame
	void Update () {

        //getAimSquare();
        //_aimObject = null;

        //http://answers.unity3d.com/questions/252847/check-if-raycast-hits-layer.html

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);

        GameObject lastTickAimObject = aimObject;

        // Get our aim object
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("BoardSquare")))
        {
            if (hit.transform.gameObject && hit.transform.gameObject != lastTickAimObject) {
                _aimObject = hit.transform.gameObject;
            }
        }
        else
        {
            _aimObject = null;
        }

        //Set BoardSquare focus highlights.
        if (aimObject != lastTickAimObject)
        {
            if(lastTickAimObject != null)
            {
                lastTickAimObject.GetComponent<BoardSquare>().setFocused(false);
            }
            if (aimObject != null)
            {
                aimObject.GetComponent<BoardSquare>().setFocused(true);
            }
        }

        //Check for inputs.
        if(Input.GetMouseButtonDown(0) && aimObject != null)
        {
            RealPlayer realPlayer = aimingPlayer.GetComponent<RealPlayer>();
            BoardSquare clickedSquare = aimObject.GetComponent<BoardSquare>();
            realPlayer.playerClickedSquare(clickedSquare);
        }
    }

    

    /// <summary>
    /// Returns the object the aimer is looking at.
    /// </summary>
    /// <returns></returns>
    public GameObject getAimObject()
    {
        return _aimObject;
    }
}
