using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Messager : MonoBehaviour {


    void Start()
    {
        displayMessage("Hello World!");
    }

    public void displayMessage(string message)
    {
        GetComponent<Text>().text = message;
    }
}
