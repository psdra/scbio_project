using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MonoBehaviour
{
    void Start()
    {
        Transform squareTransform = GetComponent<Transform>();
        SpriteRenderer squareRenderer = GetComponent<SpriteRenderer>();

        Vector3 position = squareTransform.position;
        Vector2 size = squareRenderer.bounds.size;

        Debug.Log("Position: " + position);
        Debug.Log("Size: " + size);
    }
}