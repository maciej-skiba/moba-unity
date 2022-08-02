using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private bool isCameraLocked;

    public float cameraSpeed = 20f;
    public float cameraMargin = 10f;

    private void Start()
    {
        isCameraLocked = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = isCameraLocked ? transform.position
                : new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - 4);
            isCameraLocked = isCameraLocked ? false : true;
        }
    }
    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;


        //if (Input.mouseScrollDelta.y > 0)
        //{
        //    Debug.Log("Scroll value: " +Input.mouseScrollDelta.y * transform.forward);
        //    cameraPosition += Input.mouseScrollDelta.y * transform.forward;
        //}
        //else if(Input.mouseScrollDelta.y < 0)
        //{
        //    Debug.Log("Scroll value: " +Input.mouseScrollDelta.y * transform.forward);

        //    cameraPosition += Input.mouseScrollDelta.y * transform.forward;
        //}

        if (isCameraLocked)
        {
            transform.position = new Vector3(target.transform.position.x, cameraPosition.y, target.transform.position.z - 10);
        }
        else
        {
            if(Input.mousePosition.y > Screen.height - cameraMargin)
            {
                cameraPosition.z += cameraSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.y < cameraMargin)
            {
                cameraPosition.z -= cameraSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x > Screen.width - cameraMargin)
            {
                cameraPosition.x += cameraSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x < cameraMargin)
            {
                cameraPosition.x -= cameraSpeed * Time.deltaTime;
            }
            transform.position = cameraPosition;
        }
    }
}
