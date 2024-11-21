using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AgentFactoryTest))]
public class AgentFactoryTest_Editor : Editor
{
    void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Agents in this level"))
        {
            AgentFactoryTest script = (AgentFactoryTest)target;
            //script.GenerateAgentsInTheLevel();
        }
    }
}
