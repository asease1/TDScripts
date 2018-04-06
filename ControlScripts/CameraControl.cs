using UnityEngine;
using System.Collections;
#pragma warning disable
public class CameraControl : MonoBehaviour
{
    private GameObject camera;
    public float cameraMovementSpeed, scrollSpeed;
    private float cameraCurrentPos;
    private int CurrentRotationPos;
    public float minHeight, maxHeight, minDistance, maxDistance, cameraSlowDown, rotationSpeed;
    [Tooltip("Less space then the grid size")]
    public float bufferZone;
    private float scrollAmount, currentScrollSpeed, currentHeight = 0;
    private float minX, maxX, minZ, maxZ;
    public bool Test;

    private Vector2 velocity;
    // Use this for initialization
    void Start()
    {
        minX = Grid.instance.StartPosition.x + bufferZone;
        maxX = Grid.instance.StartPosition.x +  Grid.instance.gridSize.x - bufferZone;
        minZ = Grid.instance.StartPosition.z + bufferZone;
        maxZ = Grid.instance.StartPosition.z +  Grid.instance.gridSize.y - bufferZone;
        camera = Camera.main.gameObject;
        camera.transform.parent = transform;
        camera.transform.localPosition = new Vector3(0, minHeight, minDistance);
        camera.transform.LookAt(transform);
        StartCoroutine("GetHeight", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        #region Inputs
        if (Input.GetButtonDown("RotateLeft"))
        {
            CurrentRotationPos += 90;
        }
        else if (Input.GetButtonDown("RotateRight"))
        {
            CurrentRotationPos -= 90;
        }
        if (Input.GetButton("MoveForward"))
            velocity.x = 1;
        else if (Input.GetButton("MoveBackward"))
            velocity.x = -1;
        else
            velocity.x = 0;

        if (Input.GetButton("MoveLeft"))
            velocity.y = -1;
        else if (Input.GetButton("MoveRight"))
            velocity.y = 1;
        else
            velocity.y = 0;

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            scrollAmount = Input.GetAxis("Mouse ScrollWheel") / 100;
            cameraCurrentPos -= scrollAmount * scrollSpeed;
            currentScrollSpeed = scrollAmount;
        }
        #endregion
        #region movement
        velocity = velocity.normalized;
        Vector3 caremaForward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
        transform.position += (caremaForward * velocity.x + camera.transform.right * velocity.y) * cameraMovementSpeed * Time.deltaTime;
        Vector3 tempPosition = transform.position;
        tempPosition.x = Mathf.Clamp(tempPosition.x, minX, maxX);
        tempPosition.z = Mathf.Clamp(tempPosition.z, minZ, maxZ);
        transform.position = tempPosition;
        #endregion
        #region Scroll Slow Down
        if (currentScrollSpeed != 0)
        {
            cameraCurrentPos -= currentScrollSpeed * scrollSpeed;
            currentScrollSpeed = currentScrollSpeed * (1f - cameraSlowDown * Time.deltaTime);
            if (Mathf.Abs(currentScrollSpeed) < Mathf.Abs(scrollAmount * 0.2f))
                currentScrollSpeed = 0;
            cameraCurrentPos = Mathf.Clamp01(cameraCurrentPos);
            camera.transform.localPosition = new Vector3(0, minHeight, minDistance) + new Vector3(0, maxHeight - minHeight, maxDistance - minDistance) * cameraCurrentPos;
            camera.transform.LookAt(transform);
        }
        #endregion
        #region rotation
        if (!Test)
            transform.eulerAngles = new Vector3(0, CurrentRotationPos, 0);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, CurrentRotationPos, 0), Time.deltaTime * rotationSpeed);
        #endregion
    }

    IEnumerator GetHeight(float delay)
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 20, -Vector3.up, out hit, 100.0f, LayerMask.GetMask("BuildLayer")))
            {
                currentHeight = hit.point.y;
                StopCoroutine("MoveHeight");
                StartCoroutine("MoveHeight");
            }
            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator MoveHeight()
    {
        float myHeight = transform.position.y;
        float heightDiff = currentHeight - myHeight;
        float procentY = 0;
        float thisTime = 0;
        while (true)
        {
            thisTime += 2.2f*Time.deltaTime;
            procentY = Mathf.Pow(thisTime, 2);
            procentY= Mathf.Clamp01(procentY);
            transform.position = new Vector3(transform.position.x, myHeight+heightDiff*procentY, transform.position.z);

            if (thisTime > 1)
                break;
            yield return null;
        }
    }
}