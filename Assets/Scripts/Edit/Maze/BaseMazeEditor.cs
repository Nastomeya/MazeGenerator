using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(BaseMaze))]
public abstract class BaseMazeEditor : Editor
{
    protected BaseMaze maze;
    private List<MazeObjsSetData> objectRawDatas;
    protected List<MazeObjsSetData> ObjectRawDatas
    {
        get
        {
            if (objectRawDatas == null)
            {
                if (!IsMazeSettingValid())
                    return null;
                objectRawDatas = maze.MazeObjectSetting.ObjectDatas;

            }
            return objectRawDatas;
        }
    }
    protected abstract void OneTileByOneButtons();
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawUIElements();
    }
    private bool IsMazeSettingValid()
    {
        if (maze == null)
            return false;
        if (maze.MazeObjectSetting == null)
            return false;
        if (maze.MazeObjectSetting.ObjectDatas == null)
            return false;
        return true;
    }
    protected void DrawUIElements()
    {
        if (!IsMazeSettingValid())
            return;

        CheckBoxes();

        var tiles = maze.GetTiles();
        if (tiles?.Count > 0)
            RearrangmentButtons();
    }

    private void CheckBoxes()
    {
        if (objectRawDatas == null || objectRawDatas.Count < 1)
            return;

        GUILayout.Label("Set the Maze Object to be shown as below");
        for (int i = 0; i < objectRawDatas.Count; i++)
        {
            Assert.IsNotNull(objectRawDatas[i].MazeObjects);
            if (objectRawDatas[i].MazeObjects == null)
                continue;
            var _isEnable = EditorGUILayout.Toggle(objectRawDatas[i].MazeObjects.transform.name, objectRawDatas[i].ActiveSelf);
            objectRawDatas[i].SetActive(_isEnable);
        }
    }
    protected void RearrangmentButtons()
    {
        if (GUILayout.Button("Display Index Num", GUILayout.Height(50)))
            maze.OnButtonToCreateNewMaze(true);

        if (GUILayout.Button("Display Squence", GUILayout.Height(50)))
            maze.OnButtonToCreateNewMaze(false);
        OneTileByOneButtons();
    }
}