using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float smoothFollow;
    public Transform target;

    private Vector3 velocity;
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), ref velocity, smoothFollow);
    }
}
