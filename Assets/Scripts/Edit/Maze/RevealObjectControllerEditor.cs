using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RevealObjectController))]
public class RevealObjectControllerEditor : Editor
{
    private string isOnText = "is On";
    private string isOffText = "is Off";
    public override void OnInspectorGUI()
    {
        RevealObjectController revealObjectController = (RevealObjectController)target;

        var isTextOnOffStatus = revealObjectController.isTextOnOff ? isOnText : isOffText;
        if (GUILayout.Button("The Text " + isTextOnOffStatus, GUILayout.Height(50)))
            revealObjectController.ShowTextOnOff();

        var isMarkOnOffStatus = revealObjectController.isMarkOnOff ? isOnText : isOffText;
        if (GUILayout.Button("The Mark " + isMarkOnOffStatus, GUILayout.Height(50)))
            revealObjectController.ShowMarkOnOff();
    }
}

