using System;
using System.Collections;
using UnityEngine;

public class FlyingCamera : MonoBehaviour
{
    [SerializeField]
    private float radius;
    
    private float angle;

    private float xPosition, yPosition;

    private Vector3 cameraPosition;

    private void Awake() 
    {
        cameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        angle += Time.deltaTime * 1.0f;

        xPosition = float.Parse((Math.Sin(angle) * radius).ToString());
        yPosition = float.Parse((Math.Cos(angle) * radius).ToString());

        Vector3 newCameraPosition =  new Vector3(cameraPosition.x + xPosition, cameraPosition.y + yPosition, cameraPosition.z);

        Camera.main.transform.position = newCameraPosition;

        if(angle >= 360)
        {
            angle = 0;
        }
    }

}
