using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class LSystemTurtle : MonoBehaviour 
{
    [SerializeField]
    private LSystem lSystem;

    [SerializeField]
    [Range(1, 100)]
    private float lineLength = 5.0f;

    [SerializeField]
    [Range(-100, 100)]
    public float angle = 10;

    [SerializeField]
    [Range(1,5)]
    private int numberOfGenerations = 1;

    private LineRenderer lineRenderer;

    private int currentLine = 0;

    private LSystemState state = new LSystemState();

    private Stack<LSystemState> savedState = new Stack<LSystemState>();

    private List<GameObject> lines = new List<GameObject>();

    void Awake() 
    {
        Generate();
    }

    public void Generate(bool clean = false)
    {
        // save original sentence
        lSystem.SaveOriginalSentence();

        if(clean) CleanExistingLSystem();

        if (lSystem == null)
        {
            Debug.LogError("You must have an lSystem defined");
            enabled = false;
        }
        if (lSystem.RuleCount == 0)
        {
            Debug.LogError("You must have at least one rule defined");
            enabled = false;
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        for(int i = 0; i < numberOfGenerations; i++)
        {
            savedState.Push(state.Clone());

            lSystem.Generate();

            state = savedState.Pop();
        }

        DrawLines();
    }

    void Line()
    {
        var lineGo = new GameObject($"Line_{currentLine}");
        lineGo.transform.position = Vector3.zero;
        lineGo.transform.parent = transform;

        lines.Add(lineGo);

        LineRenderer newLine = SetupLine(lineGo);
        
        // Note: transform.position.x and y is for offset when multiple trees are placed
        // first point
        newLine.SetPosition(0, new Vector3(state.x + transform.position.x, state.y + transform.position.y, 0));        
        
        CheckAngles();

        // second point
        newLine.SetPosition(1, new Vector3(state.x + transform.position.x, state.y + transform.position.y, 0));

        currentLine++;
    }   


    private void CleanExistingLSystem() 
    {
        lSystem.RestoreToOriginalSentence();

        savedState.Clear();

        foreach(GameObject line in lines)
        {
            DestroyImmediate(line, true);
        }
    }

    void Translate() => CheckAngles();

    private void CheckAngles()
    {
        if (state.angle != 0)
        {
            state.x += float.Parse((Math.Sin(state.angle / 100)).ToString());
            state.y += float.Parse((Math.Cos(state.angle / 100)).ToString());
        }
        else
        {
            state.y = state.y + state.size;
        }
    }   

    void DrawLines()
    {
        state = new LSystemState()
        {
            x = 0,
            y = 0,
            size = lineLength,
            angle = 0
        };

        string sentence = lSystem.GeneratedSentence;
        for (int i = 0; i < sentence.Length; i++) 
        {
            char c = sentence[i];
            switch(c)
            {
                case 'F':
                Line();
                break;
                case 'G':
                Translate();
                break;
                case '+':
                state.angle += angle;
                break;
                case '-':
                state.angle -= angle;
                break;
                case '[':
                savedState.Push(state.Clone());
                break; 
                case ']':
                state = savedState.Pop();
                break;                
            }
        }
    }

    private LineRenderer SetupLine(GameObject lineGo)
    {
        var newLine = lineGo.AddComponent<LineRenderer>();
        newLine.useWorldSpace = true;
        newLine.positionCount = 2;
        newLine.tag = "Line";
        newLine.material = lineRenderer.material;
        newLine.startColor = lineRenderer.startColor;
        newLine.endColor = lineRenderer.endColor;
        newLine.startWidth = lineRenderer.startWidth;
        newLine.endWidth = lineRenderer.endWidth;
        newLine.numCapVertices = 5;
        return newLine;
    }
}