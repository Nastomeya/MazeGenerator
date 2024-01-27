#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(SingleTileInDebugMode))]
public class SingleTileInDebugModeEditor : Editor
{
    private bool[] enableButtons;
    private List<IMazeObjects> wallTypes;
    private SingleTileInDebugMode singleTileInDebugMode;
    private string objectFormation;
    private void Awake()
    {
        singleTileInDebugMode = (SingleTileInDebugMode)target;
        singleTileInDebugMode.InitTile();
        wallTypes = singleTileInDebugMode.MazeObjects;

        if (wallTypes != null && enableButtons == null)
        {
            enableButtons = new bool[wallTypes.Count];
            for (int i = 0; i < enableButtons.Length; i++)
                enableButtons[i] = false;

            objectFormation = singleTileInDebugMode.GetMazeFormation().ToString();
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (wallTypes != null)
        {
            DrawUILine(Color.gray, 1);
            HideAndShowObjectButtons();
            DrawUILine(Color.gray, 1);
            FormationButton();
        }
    }
    private void HideAndShowObjectButtons()
    {
        for (int i = 0; i < wallTypes.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUI.enabled = true;
            GUILayout.Label(wallTypes[i].transform.name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            var style = new GUIStyle(GUI.skin.button);
            ProceedingHideAndShowButton(i, style, true);
            ProceedingHideAndShowButton(i, style, false);
            GUILayout.EndHorizontal();
        }
    }
    private void FormationButton()
    {
        GUI.enabled = true;

        GUI.backgroundColor = Color.white;
        GUILayout.Label("Next Wall Formation: ");
        GenerateButton("Enter To Change", () => ChangeToNextWall());

        ///////////////////////////////////////////////////////////
        GUI.backgroundColor = Color.green;
        objectFormation = EditorGUILayout.TextField("Manually Change: ", objectFormation);
        Action changeWallJob = () =>
        {
            singleTileInDebugMode.ChangeMazeFormation(int.Parse(objectFormation));
        };
        GenerateButton("Enter To Change", changeWallJob);
    }
    private void ChangeToNextWall()
    {
        singleTileInDebugMode.ChangeMazeFormation();
        objectFormation = singleTileInDebugMode.GetMazeFormation().ToString();
    }
    private void ProceedingHideAndShowButton(int index, GUIStyle style, bool isHidden)
    {
        string buttonName;
        bool isObjetShown;

        if (isHidden)
        {
            GUI.enabled = !enableButtons[index];
            buttonName = "Hide";
            isObjetShown = false;
        }
        else
        {
            GUI.enabled = enableButtons[index];
            buttonName = "Show";
            isObjetShown = true;
        }

        if (GUI.enabled)
        {
            GUI.backgroundColor = Color.yellow;
            style.normal.textColor = Color.red;
        }
        else
        {
            GUI.backgroundColor = Color.gray;
            style.normal.textColor = Color.black;
        }

        Action doJob = () =>
        {
            singleTileInDebugMode.EnableTheMazeObject(index, isObjetShown);
            enableButtons[index] = isHidden;
        };
        GenerateButton(buttonName, doJob, style);
    }
    private void GenerateButton(string buttonName, Action doJob, GUIStyle style = null)
    {
        var isButtonClicked = style == null ? GUILayout.Button(buttonName, GUILayout.Height(50)) : GUILayout.Button(buttonName, style, GUILayout.Height(50));
        if (isButtonClicked)
        {
            //CleanConsoleInEditor.ClearLogConsole();
            doJob();
        }
    }

    private void DrawUILine(Color color, int thickness = 2, int padding = 10)//ref: https://forum.unity.com/threads/horizontal-line-in-editor-window.520812/
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
#endif