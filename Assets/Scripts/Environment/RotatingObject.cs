using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("The minimum axis in euler to rotate around")]
    public Vector3 minDirection = Vector3.up;
    [Tooltip("The maximum axis in euler to rotate around")]
    public Vector3 maxDirection = Vector3.up;

    [Space(10)]
    [Tooltip("The minimum speed with which to rotate")]
    public float minSpeed = 25;
    [Tooltip("The maximum speed with which to rotate")]
    public float maxSpeed = 25;

    [Space(10)]
    [Tooltip("Randomize speed and direction only at void::Start")]
    public bool randomizeOnce = true;


    [Header("Internals")]
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    float speed;

    private void Start()
    {
        direction = new Vector3(
            Random.Range(minDirection.x, maxDirection.x),
            Random.Range(minDirection.y, maxDirection.y),
            Random.Range(minDirection.z, maxDirection.z));
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update () {

        if (!randomizeOnce) { 
            direction = new Vector3(
                Random.Range(minDirection.x, maxDirection.x),
                Random.Range(minDirection.y, maxDirection.y),
                Random.Range(minDirection.z, maxDirection.z));

            speed = Random.Range(minSpeed, maxSpeed);
        }
        Rotate(direction);
        
    }



    void Rotate(Vector3 newDirection)
    {
        Quaternion currentRotation = transform.rotation;

        transform.RotateAround(transform.position, newDirection, speed * Time.deltaTime);

    }
}
