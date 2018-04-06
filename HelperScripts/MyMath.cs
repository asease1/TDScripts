using UnityEngine;
using System.Collections;

public static class MyMath {

	public static bool InterSectLine(Vector3 startPoint, Vector3 endPoint, Vector3[] pointOfInterSect)
    {
        for (int i = 0; i < pointOfInterSect.Length; i++)
        {
            float interNumber = Vector2.Distance(startPoint, pointOfInterSect[i]) + Vector2.Distance(endPoint, pointOfInterSect[i]) - Vector2.Distance(startPoint, endPoint);
            if (Mathf.Abs(interNumber) < 0.5f)
                return true;
        }
        Debug.Log("Nop");  
        return false;
    }
}
