using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour
{
    // Start is called before the first frame update

    Transform gravityTransform, playerTransform;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerTransform = GameObject.Find("Player").transform;
        // transform.position = pt.position;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = playerTransform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, gravityTransform.rotation, 10 * Time.deltaTime);
    }

}
