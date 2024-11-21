using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace N_Awakening.DungeonCrawler
{
    [CustomEditor(typeof(Agent))]
    public class AgentEditor : Editor
    {
        //void Update() for the rendering of the Inspector
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            DrawDefaultInspector();
            if (GUILayout.Button("Preeeessss Meeeee!!!!!"))
            {
                Debug.LogWarning("I have been PREEESSSSSEED");
                Agent agent = (Agent)target;
                agent.InitializeAgent();
            }

        }
    }
}