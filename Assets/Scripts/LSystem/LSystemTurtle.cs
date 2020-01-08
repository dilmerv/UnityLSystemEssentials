
using System;
using UnityEngine;
using System.Collections.Generic;
using DilmerGames.Core.Utilities;

[RequireComponent(typeof(LineRenderer))]
public class LSystemTurtle : MonoBehaviour 
{
    [SerializeField]
    private LSystem lSystem;

    [SerializeField]
    [Range(1, 100)]
    public float lineLength = 5.0f;

    [SerializeField]
    [Range(0.05f, 0.30f)]
    public float lineWidth = 0.1f;

    [SerializeField]
    [Range(0.2f, 0.4f)]
    public float leafWidth = 0.2f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    public float leafLength = 0.5f;

    [SerializeField]
    [Range(-100, 100)]
    public float angle = 10;

    [SerializeField]
    public bool ignoreZ = true;
    
    [HideInInspector]
    public bool generateRandomMaterial = false;
    
    [HideInInspector]
    public bool generateMultipleMaterial = false;
    
    [SerializeField]
    [Range(1,5)]
    public int numberOfGenerations = 1;

    [SerializeField]
    private Color leafColor;

    [SerializeField]
    private Color lineColor;

    private LineRenderer lineRenderer;

    private int currentLine = 0;

    private LSystemState state = new LSystemState();

    private Stack<LSystemState> savedState = new Stack<LSystemState>();

    private List<GameObject> lines = new List<GameObject>();

    private Material randomMaterial;

    private void Start() 
    { 
        leafColor.a = 1;
        lineColor.a = 1;
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

        if(generateRandomMaterial)
        {
            GenerateRandomMaterial();
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

    void Line(String s)
    {   
        var lineGo = new GameObject($"{s}_{currentLine}");
        lineGo.transform.position = Vector3.zero;
        lineGo.transform.parent = transform;
        
        lines.Add(lineGo);

        if (generateMultipleMaterial)
        {
            GenerateRandomMaterial();
        }

        lineGo.tag = s.Equals("Line") ? "Line" : "Leaf";

        LineRenderer newLine = SetupLine(lineGo);
        
        // Note: transform.position.x and y is for offset when multiple trees are placed
        // first point
        if (s.Equals("Line"))
        {
            newLine.SetPosition(0, new Vector3(state.x + transform.position.x, state.y + transform.position.y, ignoreZ ? transform.position.z : state.z + transform.position.z));        
            
            CheckAngles();

            // second point
            newLine.SetPosition(1, new Vector3(state.x + transform.position.x, state.y + transform.position.y, ignoreZ ? transform.position.z : state.z + transform.position.z));
        }
        else if (s.Equals("Leaf"))
        {
            lineGo.tag = "Leaf";

            newLine.positionCount = 3;
            
            // Note: transform.position.x and y is for offset when multiple trees are placed
            // first point
            newLine.SetPosition(0, new Vector3(state.x + transform.position.x, state.y + transform.position.y, ignoreZ ? transform.position.z : state.z + transform.position.z));        
            
            CheckAngles();

            // second point
            newLine.SetPosition(2, new Vector3(leafLength + state.x + transform.position.x, leafLength + state.y + transform.position.y, ignoreZ ? transform.position.z : state.z + transform.position.z));
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, leafWidth);
            curve.AddKey(lineLength * (1.5f / 3.0f), leafWidth * 3.0f);
            curve.AddKey(lineLength, leafWidth * 0.25f);

            newLine.startColor = leafColor;
            newLine.endColor = leafColor;
            newLine.widthCurve = curve;

            Vector3[] positions = new Vector3[newLine.positionCount];
            newLine.GetPositions(positions);
            newLine.SetPosition(1, (newLine.GetPosition(0) + newLine.GetPosition(2)) / 2.0f );    
        }

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
            state.z += float.Parse((Math.Cos(state.angle / 100)).ToString());
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
            z = 0,
            size = lineLength,
            angle = state.angle
        };

        string sentence = lSystem.GeneratedSentence;
        for (int i = 0; i < sentence.Length; i++) 
        {
            char c = sentence[i];
            switch(c)
            {
                case 'F':
                Line("Line");
                break;
                case 'G':
                Translate();
                break;
                case 'J':
                Line("Leaf");
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

    private void GenerateRandomMaterial()
    {
        randomMaterial = MaterialUtils.CreateMaterialWithRandomColor($"{gameObject.name}_material");
    }

    private LineRenderer SetupLine(GameObject lineGo)
    {
        var newLine = lineGo.AddComponent<LineRenderer>(); 
        newLine.useWorldSpace = true;
        newLine.positionCount = 2;
        newLine.startWidth = lineWidth;
        newLine.endWidth = lineWidth;
        newLine.material = generateRandomMaterial ? randomMaterial : lineRenderer.material;
        newLine.startColor = lineColor;
        newLine.endColor = lineColor;
        newLine.numCapVertices = 5;
        return newLine;
    }
}