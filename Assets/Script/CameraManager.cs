using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject rot;
    Quaternion targetRot;

    void Start()
    {
        rot = GameObject.Find("Rot");

    }

    // Update is called once per frame
    void Update()
    {
        if (targetRot != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot.transform.rotation, 10 * Time.deltaTime);
        }
    }

    public void CameraRot()
    {
        targetRot = rot.transform.rotation;
    }


}
