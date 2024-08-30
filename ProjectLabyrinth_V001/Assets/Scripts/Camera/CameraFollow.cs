using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 0.12f;
    Vector3 smoothSpeed;
    Vector3 currentRotation;

    public bool lockCursor;

    public float offset = 5;

    public float mouseSense = 10;

    public Vector2 boundary = new Vector2(-2, 40);

    float mouseX;
    float mouseY;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
 
    }
    
    void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSense;
        mouseY += Input.GetAxis("Mouse Y") * mouseSense;
        mouseY =  Mathf.Clamp(mouseY, boundary.x, boundary.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref smoothSpeed, smoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * offset;


        transform.LookAt(target.transform);

    }

    /* Test Code
     * 
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    *
    */

}
