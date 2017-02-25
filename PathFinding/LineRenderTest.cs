using UnityEngine;
using System.Collections;

public class LineRenderTest : MonoBehaviour {
    private Grid gridClass;
    public GameObject lineRender;
	// Use this for initialization
	void Start () {
        Transform lineRenderGrouppe = new GameObject("LineRenders").transform;
        gridClass = GetComponent<Grid>();
        foreach(Node elm in gridClass.grid)
        {
            GameObject temp = Instantiate(lineRender, elm.worldPosition, Quaternion.identity) as GameObject;
            temp.transform.parent = lineRenderGrouppe;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
