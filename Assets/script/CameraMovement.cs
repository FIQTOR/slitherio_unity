using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float smoothFollow;
    public Transform target;

    private Vector3 velocity;
    private float cameraOrtographicSize;
    void Awake()
    {
        cameraOrtographicSize = GetComponent<Camera>().orthographicSize;
    }
    void FixedUpdate()
    {
        GetComponent<Camera>().orthographicSize = cameraOrtographicSize + target.localScale.y;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), ref velocity, smoothFollow);
    }
}
