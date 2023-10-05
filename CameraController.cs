using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    //IMP - Zoom Limit
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameplayState == GAMEPLAY_STATE.PLACEMENT 
        || GameManager.instance.gameplayState == GAMEPLAY_STATE.PHOTO){
            HandleMouseInput();
            HandleMovementInput();
        }
    }

    void HandleMouseInput(){
        if(Input.GetMouseButtonDown(1)){
            rotateStartPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(1)){
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 15f));
        }
    }

    void HandleMovementInput(){
        if(Input.GetKey(KeyCode.Q)){
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if(Input.GetKey(KeyCode.E)){
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if(Input.GetKey(KeyCode.R)){
            newZoom += zoomAmount;
        }

        if(Input.GetKey(KeyCode.F)){
            newZoom -= zoomAmount;
        }
        
        newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);
        newZoom.z = Mathf.Clamp(newZoom.z, minZ, maxZ);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom,  Time.deltaTime * movementTime);
    }
}
