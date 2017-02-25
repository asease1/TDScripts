using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

    public LayerMask wallLayer;
    public GameObject panelClosed, panelFlat, panelOpen, panelLeft, panelRight;
    private GameObject forwardPanel, backwardPanel, leftPanel, rightPanel;
    // Use this for initialization
    void Start()
    {
        PlacePanels(true);
    }

    public void UpdatePannels()
    {
        Destroy(forwardPanel);
        Destroy(backwardPanel);
        Destroy(leftPanel);
        Destroy(rightPanel);
        PlacePanels(false);
    }

    private void PlacePanels(bool callHits)
    {
        float gridSize = Grid.instance.cubeSize;
        RaycastHit hit;
        bool forwardCollade = Physics.Raycast(transform.position + transform.forward * gridSize + Vector3.up * 20, -Vector3.up, out hit, 100.0f, wallLayer);
        if (forwardCollade && callHits)       
            hit.collider.gameObject.GetComponent<WallScript>().UpdatePannels();    
        bool backWardCollade = Physics.Raycast(transform.position + -transform.forward * gridSize + Vector3.up * 20, -Vector3.up, out hit, 100.0f, wallLayer);
        if (backWardCollade && callHits)
            hit.collider.gameObject.GetComponent<WallScript>().UpdatePannels();
        bool rightCollade = Physics.Raycast(transform.position + transform.right * gridSize + Vector3.up * 20, -Vector3.up, out hit, 100.0f, wallLayer);
        if (rightCollade && callHits)
            hit.collider.gameObject.GetComponent<WallScript>().UpdatePannels();
        bool leftCollade = Physics.Raycast(transform.position + -transform.right * gridSize + Vector3.up * 20, -Vector3.up, out hit, 100.0f, wallLayer);
        if (leftCollade && callHits)
            hit.collider.gameObject.GetComponent<WallScript>().UpdatePannels();

        if (forwardCollade)
        {
            Transform temp = (Instantiate(panelFlat, transform.position, Quaternion.identity) as GameObject).transform;
            temp.parent = transform;
            temp.localRotation = Quaternion.Euler(-90, 0, 180);
            forwardPanel = temp.gameObject;
        }
        else
        {
            Transform temp = transform;
            switch (rightCollade)
            {
                case true:
                    switch (leftCollade)
                    {
                        case true:
                            temp = (Instantiate(panelClosed, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 180);
                            break;
                        case false:
                            temp = (Instantiate(panelRight, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 180);
                            break;
                    }
                    break;
                case false:
                    switch (leftCollade)
                    {
                        case true:
                            temp = (Instantiate(panelLeft, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 180);
                            break;
                        case false:
                            temp = (Instantiate(panelOpen, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 180);
                            break;
                    }
                    break;
            }
            forwardPanel = temp.gameObject;
        }
        if (backWardCollade)
        {
            Transform temp = (Instantiate(panelFlat, transform.position, Quaternion.identity) as GameObject).transform;
            temp.parent = transform;
            temp.localRotation = Quaternion.Euler(-90, 0, 0);
            backwardPanel = temp.gameObject;
        }
        else
        {
            Transform temp = transform;
            switch (rightCollade)
            {
                case true:
                    switch (leftCollade)
                    {
                        case true:
                            temp = (Instantiate(panelClosed, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 0);
                            break;
                        case false:
                            temp = (Instantiate(panelLeft, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 0);
                            break;
                    }
                    break;
                case false:
                    switch (leftCollade)
                    {
                        case true:
                            temp = (Instantiate(panelRight, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 0);
                            break;
                        case false:
                            temp = (Instantiate(panelOpen, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 0);
                            break;
                    }
                    break;
            }
            backwardPanel = temp.gameObject;
        }
        if (rightCollade)
        {
            Transform temp = (Instantiate(panelFlat, transform.position, Quaternion.identity) as GameObject).transform;
            temp.parent = transform;
            temp.localRotation = Quaternion.Euler(-90, 0, -90);
            rightPanel = temp.gameObject;
        }
        else
        {
            Transform temp = transform;
            switch (forwardCollade)
            {
                case true:
                    switch (backWardCollade)
                    {
                        case true:
                            temp = (Instantiate(panelClosed, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, -90);
                            break;
                        case false:
                            temp = (Instantiate(panelLeft, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, -90);
                            break;
                    }
                    break;
                case false:
                    switch (backWardCollade)
                    {
                        case true:
                            temp = (Instantiate(panelRight, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, -90);
                            break;
                        case false:
                            temp = (Instantiate(panelOpen, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, -90);
                            break;
                    }
                    break;
            }
            rightPanel = temp.gameObject;
        }
        if (leftCollade)
        {
            Transform temp = (Instantiate(panelFlat, transform.position, Quaternion.identity) as GameObject).transform;
            temp.parent = transform;
            temp.localRotation = Quaternion.Euler(-90, 0, 90);
            leftPanel = temp.gameObject;
        }
        else
        {
            Transform temp = transform;
            switch (forwardCollade)
            {
                case true:
                    switch (backWardCollade)
                    {
                        case true:
                            temp = (Instantiate(panelClosed, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 90);
                            break;
                        case false:
                            temp = (Instantiate(panelRight, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 90);
                            break;
                    }
                    break;
                case false:
                    switch (backWardCollade)
                    {
                        case true:
                            temp = (Instantiate(panelLeft, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 90);
                            break;
                        case false:
                            temp = (Instantiate(panelOpen, transform.position, Quaternion.identity) as GameObject).transform;
                            temp.parent = transform;
                            temp.localRotation = Quaternion.Euler(-90, 0, 90);
                            break;
                    }
                    break;
            }
            leftPanel = temp.gameObject;
        }
    }
}
