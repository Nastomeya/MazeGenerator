using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RevealObjectController))]
public class RevealObjectControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RevealObjectController revealObjectController = (RevealObjectController)target;
        if (GUILayout.Button("On or Off The Objects", GUILayout.Height(50)))
            revealObjectController.ShowItOnOff();
    }
}

