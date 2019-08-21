using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField,Range(0f,200f)]
    float minHeight,maxHeight;

    [SerializeField,Range(0f,1f)]
    float scrollSensitivity = 0.3f;
    [SerializeField, Range(0f, 60f)]
    float rotationSpeed = 60f;

    float xRotation;
    float cameraHeight;
    float heightOffset;

    bool firstFrame;

    Vector2 screenOrigin;
    Vector2 screenDrag;

    Vector2 offset;

    Vector3 cameraOrigin;

    Vector3 forward;
    Vector3 right;

    Vector3 movementVector;

    [SerializeField,Range(0,20f)]
    float cameraSpeed = 10f;

    [SerializeField]
    bool controlWithMouse = false;

    // Start is called before the first frame update
    public void Set_Opt(float min,float max,float rot,float speed,float sensitivity)
    {
        minHeight = min;
        maxHeight = max;
        rotationSpeed = rot;
        cameraSpeed = speed;
        scrollSensitivity = sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        heightOffset -= Input.GetAxisRaw("Mouse ScrollWheel") * scrollSensitivity;
        heightOffset = Mathf.Clamp(heightOffset, 0f, 1f);
        cameraHeight = Mathf.Lerp(minHeight, maxHeight, heightOffset);

        movementVector = (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal"));
        movementVector = Vector3.ProjectOnPlane(movementVector, Vector3.up).normalized;
        transform.position += movementVector * cameraSpeed * Time.deltaTime * Mathf.Sqrt(cameraHeight);

        if (Input.GetKey(KeyCode.LeftAlt)&&Input.GetKey(KeyCode.Mouse0))
        {
            transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0f), Space.World);
        }

        if (controlWithMouse && Input.GetKey(KeyCode.Mouse2))
        {
            if (firstFrame)
            {
                screenOrigin = Input.mousePosition;
                cameraOrigin = transform.position;
            }
            else
            {
                screenDrag = Input.mousePosition;
            }
            offset = (screenDrag - screenOrigin) * -1;
            forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            right = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            //transform.position = cameraOrigin + (new Vector3(offset.x, 0, offset.y) * transform.position.y * Time.fixedDeltaTime);
            transform.position = cameraOrigin + ((forward * offset.y + right * offset.x) * transform.position.y * Time.fixedDeltaTime);
            firstFrame = false;
        }
        else
        {
            firstFrame = true;
        }
        transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);

        xRotation = Mathf.Lerp(20f, 85f, cameraHeight / maxHeight);
        transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
