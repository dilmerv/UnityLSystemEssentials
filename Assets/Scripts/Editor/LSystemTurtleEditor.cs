using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LSystemTurtle))]
public class LSystemTurtleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LSystemTurtle lSystemTurtle = (LSystemTurtle)target;

        if(GUILayout.Button("Generate"))
        {
            lSystemTurtle.Generate(clean: true);
        }
    }
}
