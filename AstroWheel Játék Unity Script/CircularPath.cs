using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPath : MonoBehaviour
{
    public Transform target; //target object to rotate around
    public float speed = 2f; //speed of the moving object
    public float radius = 1f; //radius of circular path
    public float angle = 0f; //current angle of the object

    // Update is called once per frame
    void Update()
    {
        
        //calculate the new position of the object using Mathf functions

        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        //update the position of the new object
        transform.position = new Vector3(x, y, z);

        //increment the angle to move the object along the circular path
        angle += speed * Time.deltaTime;
    }
}
