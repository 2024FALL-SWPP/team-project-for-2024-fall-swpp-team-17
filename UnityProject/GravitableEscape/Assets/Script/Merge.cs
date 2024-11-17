using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour
{
    // Start is called before the first frame update

    Transform gravityTransform;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        this.transform.Rotate(0, 0, 180, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, gravityTransform.rotation, 10 * Time.deltaTime);
    }

}
