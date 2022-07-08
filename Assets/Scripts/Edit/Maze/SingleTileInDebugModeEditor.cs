using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(SingleTileInDebugMode))]
public class SingleTileInDebugModeEditor : Editor
{
    private bool[] enableButtons;
    private List<IMazeWall> wallTypes;
    private SingleTileInDebugMode singleTileInDebugMode;
    private string wallFormation;
    private void Awake()
    {
        singleTileInDebugMode = (SingleTileInDebugMode)target;
        singleTileInDebugMode.InitTile();
        wallTypes = singleTileInDebugMode.GetWallTypes();

        if (wallTypes != null && enableButtons == null)
        {
            enableButtons = new bool[wallTypes.Count];
            for (int i = 0; i < enableButtons.Length; i++)
                enableButtons[i] = false;

            wallFormation = singleTileInDebugMode.GetWallFormation().ToString();
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
        GenerateButton("Enter To Change", () => { singleTileInDebugMode.ChangeWallFormation(); wallFormation = singleTileInDebugMode.GetWallFormation().ToString(); });

        GUI.backgroundColor = Color.green;
        wallFormation = EditorGUILayout.TextField("Manually Change: ", wallFormation);
        GenerateButton("Enter To Change", () => singleTileInDebugMode.ChangeWallFormation(int.Parse(wallFormation)));
    }
    private void ProceedingHideAndShowButton(int index, GUIStyle style, bool isHidden)
    {
        string buttonName;
        Action action;
        bool isWallShown;

        if (isHidden)
        {
            GUI.enabled = !enableButtons[index];
            buttonName = "Hide";
            isWallShown = false;
        }
        else
        {
            GUI.enabled = enableButtons[index];
            buttonName = "Show";
            isWallShown = true;
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

        action = () => { singleTileInDebugMode.EnableTheWall(index, isWallShown); enableButtons[index] = isHidden; };
        GenerateButton(buttonName, action, style);
    }
    private void GenerateButton(string buttonName, Action action, GUIStyle style = null)
    {
        var isButtonClicked = style == null ? GUILayout.Button(buttonName, GUILayout.Height(50)) : GUILayout.Button(buttonName, style, GUILayout.Height(50));
        if (isButtonClicked)
        {
            CleanConsoleInEditor.ClearLogConsole();
            action();
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
