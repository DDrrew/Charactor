using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput PI;
    public float horizontalSpeed = 100.0f;
    //private GameObject playerHandle;
    private GameObject cameraHandle;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        //playerHandle = cameraHandle.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        cameraHandle.transform.Rotate(Vector3.up, PI.Jright * horizontalSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, -PI.Jup * horizontalSpeed * Time.deltaTime);
    }
}
