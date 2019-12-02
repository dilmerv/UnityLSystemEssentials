using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LSystemTurtle))]
public class LSystemTurtleRandomizer : MonoBehaviour 
{
    [SerializeField]
    [Range(0, 20)]
    private float randomizeInSeconds = 5.0f;

    [SerializeField]
    [Range(-80, 80)]
    private float minAngle = -10;

    [SerializeField]
    [Range(-80, 80)]
    private float maxAngle = 10;

    private float randomizerTimer = 0;

    private LSystemTurtle lSystemTurtle;

    void Awake() 
    {
        lSystemTurtle = GetComponent<LSystemTurtle>();
    }

    void Update()
    {
        if(randomizerTimer >= randomizeInSeconds)
        {
            lSystemTurtle.angle = Random.Range(minAngle, maxAngle);
            lSystemTurtle.Generate(clean: true);

            randomizerTimer = 0;
        }

        randomizerTimer += Time.deltaTime * 1.0f;
    }
}