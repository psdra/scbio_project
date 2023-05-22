using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
        float speed = 500f;
        public Vector3 targetPosition;

void Awake(){
        targetPosition = transform.position;
}
    // Update is called once per frame
void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
