using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject rot;
    Quaternion targetRot;

    private float followSpeed = 15f;
    private float sensitivity = 300f;
    private float clampAngleY = 70f;

    private float rotX;
    private float rotY;

    public Transform realCam;
    public Transform objectTofollow;
    public Vector3 offset = new Vector3(0, 3, -6);

    void Start()
    {
        rot = GameObject.Find("Rot");

        Vector3 rotation = transform.localRotation.eulerAngles;
        rotX = rotation.x;
        rotY = rotation.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (targetRot != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot.transform.rotation, 10 * Time.deltaTime);
        }

        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngleY, clampAngleY);

        Quaternion rotAngle = Quaternion.Euler(rotX, rotY, 0);
        realCam.localRotation = rotAngle;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = objectTofollow.position + transform.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    public void CameraRot()
    {
        targetRot = rot.transform.rotation;
    }


}
