using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bobbing : MonoBehaviour
{
    public float speed = 1.5f; //speed of the moving object
    public float height = 4f;
    public Transform target; //target object to bob


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        //calculate the new position of the object using Mathf functions
        
        float newY = Mathf.Sin(Time.time) * speed;
        

        //increment the angle to move the object along the circular path
        transform.position = new Vector3(pos.x, newY, pos.z) * height;

    }
}
