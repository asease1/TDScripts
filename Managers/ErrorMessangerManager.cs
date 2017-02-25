using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ErrorMessangerManager : MonoBehaviour {


    public Text text;
    public GameObject Display;
    public float DisplayTimer;
    private Queue<string> listOfError = new Queue<string>();
    private static ErrorMessangerManager errorManager;

    public static ErrorMessangerManager instance { get { return errorManager; } }

    void Start()
    {
        errorManager = this;

    }

    public void DisplayError(string errorMessange)
    {
        listOfError.Enqueue(errorMessange);
        if (listOfError.Count == 1)
        {
            StartCoroutine(MessangerDisplay(DisplayTimer));
        }
    }

    IEnumerator MessangerDisplay(float timer)
    {
        string messange = listOfError.Dequeue();
        Display.SetActive(true);
        text.text = messange;
        yield return new WaitForSeconds(timer);
        Display.SetActive(false);
        if (listOfError.Count > 0)
            StartCoroutine(MessangerDisplay(DisplayTimer));

    }
	
}
