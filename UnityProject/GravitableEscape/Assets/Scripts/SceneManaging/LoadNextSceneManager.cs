using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextSceneManager : MonoBehaviour
{
    public LoadScene loadScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("collision");
            loadScene.LoadNextScene();
        }
    }
}