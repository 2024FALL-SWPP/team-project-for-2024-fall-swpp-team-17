using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotManager : MonoBehaviour
{
    Vector3 gravitydir = new Vector3(0, -10f, 0);
    private Rigidbody[] rigidbodies;
    CameraManager cameraManager;
    void Start()
    {
        rigidbodies = FindObjectsOfType<Rigidbody>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(AllRest()){

            if(Input.GetKeyDown(KeyCode.Alpha1)){
                transform.Rotate(0, 0, -90, Space.World);
                Physics.gravity = Quaternion.Euler(0, 0, -90) * gravitydir;
                gravitydir = Quaternion.Euler(0, 0, -90) * gravitydir;
                cameraManager.CameraRot();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)){
                transform.Rotate(0, 0, -180, Space.World);
                Physics.gravity = Quaternion.Euler(0, 0, -180) * gravitydir;
                gravitydir = Quaternion.Euler(0, 0, -180) * gravitydir;
                cameraManager.CameraRot();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)){
                transform.Rotate(0, 0, -270, Space.World);
                Physics.gravity = Quaternion.Euler(0, 0, -270) * gravitydir;
                gravitydir = Quaternion.Euler(0, 0, -270) * gravitydir;
                cameraManager.CameraRot();
            }
        }
    }

    private bool AllRest(){
        foreach(Rigidbody rb in rigidbodies){
            if(rb.velocity.magnitude > 0.1f){
                return false;
            }
        }
        return true;
    }

}
