using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float rotationSensitivity;
    public float movementSpeed;

    private Vector3 mousePos;

    void FixedUpdate()
    {
        RotateToMouse();
        Movement();
    }

    void RotateToMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Quaternion direction = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSensitivity * Time.deltaTime);
    }

    void Movement()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;
    }
}
