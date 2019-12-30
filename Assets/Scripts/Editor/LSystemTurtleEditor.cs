using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[CustomEditor(typeof(LSystemTurtle))]
public class LSystemTurtleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LSystemTurtle lSystemTurtle = (LSystemTurtle)target;

        lSystemTurtle.generateRandomMaterial = EditorGUILayout.Toggle("Generate Random Material", lSystemTurtle.generateRandomMaterial);  

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(lSystemTurtle.generateRandomMaterial)))
         {
             if (group.visible == true)
             {
                 EditorGUI.indentLevel++;
                 lSystemTurtle.generateMultipleMaterial = EditorGUILayout.Toggle("Multiple Colors", lSystemTurtle.generateMultipleMaterial);  
                 EditorGUI.indentLevel--;
             }
         }

        if(GUILayout.Button("Generate"))
        {
            lSystemTurtle.Generate(clean: true);
        }     
    }
}
