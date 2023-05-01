using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float rotationSensitivity;
    private float movementSpeed;
    public float normalSpeed;
    public float boostSpeed;

    private Vector3 mousePos;

    public GameObject bodyPrefab;
    private float bodyFollowSpeed;
    public float normalFollowSpeed;
    public float boostFollowSpeed;

    public float sensitivityGrowing;
    private float currentSize = 1;

    private List<Transform> BodyParts = new List<Transform>();

    void Start()
    {
        movementSpeed = normalSpeed;
        bodyFollowSpeed = normalFollowSpeed;
    }

    void FixedUpdate()
    {
        RotateToMouse();
        Movement();
        BoostMovement();
        BodyMovement();

        StartCoroutine("spawnFood");
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

    private bool Boost;
    void BoostMovement()
    {
        if(Boost != false)
        {
            if(!Input.GetMouseButton(0))
            {
                movementSpeed = normalSpeed;
                bodyFollowSpeed = normalFollowSpeed;
                Boost = false;
            }
        }
        else
        {
            if(Input.GetMouseButton(0))
            {
                movementSpeed = boostSpeed;
                bodyFollowSpeed = boostFollowSpeed;
                Boost = true;
            }
        }
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

        Growing();
    }

    void Growing()
    {
        currentSize += sensitivityGrowing;
        transform.localScale = Vector3.one * currentSize;
        
        normalFollowSpeed = (normalSpeed / 15) / currentSize;
        boostFollowSpeed = (boostSpeed / (15 + boostSpeed - normalSpeed) / currentSize);
        if(Boost != true)
        {
            movementSpeed = normalSpeed;
        }
        else
        {
            movementSpeed = boostSpeed;
        }

        foreach(Transform body in BodyParts)
        {
            body.localScale = transform.localScale;
        }

        if(rotationSensitivity > 0.5f)
        {
            rotationSensitivity -= 0.01f;
        }
    }

    public float foodSpawnTime;
    public GameObject foodPrefab;

    private IEnumerator spawnFood()
    {
        yield return new WaitForSeconds(foodSpawnTime);

        Vector2 RandomPos = new Vector2(Random.Range(transform.position.x - 20, transform.position.x + 20), Random.Range(transform.position.y - 20, transform.position.y + 20));

        Instantiate(foodPrefab, RandomPos, Quaternion.identity);

        StopCoroutine("spawnFood");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "food")
        {
            Destroy(col.gameObject);

            AddBodyParts();
        }
    }
}
