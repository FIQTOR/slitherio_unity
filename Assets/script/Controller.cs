﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float rotationSensitivity;
    public float movementSpeed;

    private Vector3 mousePos;

    public GameObject bodyPrefab;
    public float bodyFollowSpeed;

    private List<Transform> BodyParts = new List<Transform>();

    void FixedUpdate()
    {
        RotateToMouse();
        Movement();
        BodyMovement();

        if(Input.GetKey(KeyCode.F))
            AddBodyParts();
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

    void BodyMovement()
    {
        for(int i = 0; i < BodyParts.Count; i++)
        {
            if(i != 0)
            {
                BodyParts[i].position = Vector3.Lerp(BodyParts[i].position, BodyParts[i - 1].position, bodyFollowSpeed);
                BodyParts[i].up = BodyParts[i].position - BodyParts[i - 1].position;
            }
            else
            {
                BodyParts[i].position = Vector3.Lerp(BodyParts[i].position, transform.position, bodyFollowSpeed);
                BodyParts[i].up = BodyParts[i].position - transform.position;
            }
        }
    }

    void AddBodyParts()
    {
        if(BodyParts.Count != 0)
        {
            GameObject newBody = Instantiate(bodyPrefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation);
            newBody.transform.localScale = transform.localScale;
            newBody.GetComponent<SpriteRenderer>().sortingOrder = (0 - 1 - BodyParts.Count);

            BodyParts.Add(newBody.transform);
        }
        else
        {
            GameObject newBody = Instantiate(bodyPrefab, transform.position, transform.rotation);
            newBody.transform.localScale = transform.localScale;
            newBody.GetComponent<SpriteRenderer>().sortingOrder = (0 - 1 - BodyParts.Count);

            BodyParts.Add(newBody.transform);
        }
    }
}
