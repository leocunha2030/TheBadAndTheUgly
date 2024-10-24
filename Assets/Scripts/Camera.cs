using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform cameraTarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    // LateUpdate is called once per frame, after all Update calls
    void LateUpdate()
    {
        transform.position = cameraTarget.position;
        transform.rotation = cameraTarget.rotation;
    }
}