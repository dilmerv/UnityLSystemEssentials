using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private float radius = 10.0f;

    private float degreesCounter = 0;

    private void Start() 
    {
        StartCoroutine(DrawCircle());
    }
    
    IEnumerator DrawCircle()
    {
        while(degreesCounter <= 360)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            float xPosition = float.Parse(Math.Sin(degreesCounter).ToString()) * radius;
            float yPosition = float.Parse(Math.Cos(degreesCounter).ToString()) * radius;

            degreesCounter += 10;

            cube.transform.position = new Vector3(xPosition, yPosition);
        }

        yield return null;
    }

}
