using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PatronManager))]
public class EditorSpawnPatron : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PatronManager manager = (PatronManager)target;
        if (GUILayout.Button("Spawn Patron"))
        {
            manager.SpawnPatron();
        }
    }
}
