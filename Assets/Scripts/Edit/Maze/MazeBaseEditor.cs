using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeBase))]
public abstract class MazeBaseEditor : Editor
{
    protected MazeBase maze;
    protected Dictionary<int, bool> currWallTypes;
    private List<Transform> allWallPrefabs;
    private string fileNamePath;
    protected abstract void OneTileByOneButtons();

    protected void Init()
    {
        allWallPrefabs = maze.GetAllWallPrefabs();

        if (allWallPrefabs != null)
        {
            fileNamePath = maze.GetFileNameAndPath();
            maze.SetWallDataFromPrefabs();

            currWallTypes = maze.GetCurrWallTypes();
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawUIElements();
    }

    protected void DrawUIElements()
    {
        if (allWallPrefabs != null)
        {
            SaveButtonAndCheckBoxes();
            var tiles = maze.GetTiles();
            if (tiles?.Count > 0)
                RearrangmentButtons();
        }
    }

    private void SaveButtonAndCheckBoxes()
    {
        if (currWallTypes != null)
        {
            CheckBoxes();
            SaveButton();
        }
    }
    private void CheckBoxes()
    {
        GUILayout.Label("Choose The Walls To Be Shown As Below");
        for (int i = 0; i < allWallPrefabs.Count; i++)
        {
            if (!currWallTypes.ContainsKey(i))
                currWallTypes.Add(i, false);

            var _isEnable = EditorGUILayout.Toggle(allWallPrefabs[i].name, currWallTypes[i]);
            currWallTypes[i] = _isEnable;
        }
    }
    private void SaveButton()
    {
        bool isSaveButton = GUILayout.Button("Save", GUILayout.Height(50));
        if (isSaveButton)
        {
            var saveDataIsNeeded = false;
            var oldWallTypes = maze.LoadData();//shallow copy
            int i = 0;

            foreach (var currData in currWallTypes)
            {
                var hasFound = oldWallTypes.TryGetValue(currData.Key, out bool oldValue);
                if (hasFound)
                {
                    bool isDifferent = oldValue != currData.Value;
                    if (isDifferent)//found something new
                    {
                        maze.TirggerToChangeWallTypes(currData.Key, currData.Value);
                        saveDataIsNeeded = true;

                        i++;
                    }
                }
            }
            if (saveDataIsNeeded)
            {
                Utils.SaveDicToFile(currWallTypes, fileNamePath);
                maze.UpdateWallTypes(currWallTypes);
            }
        }
    }
    protected void RearrangmentButtons()
    {
        if (GUILayout.Button("Display Index Num", GUILayout.Height(50)))
            maze.OnButtonToCreateNewMaze(true);

        if (GUILayout.Button("Display Squence", GUILayout.Height(50)))
            maze.OnButtonToCreateNewMaze(false);
        CleanConsoleInEditor.ClearLogConsole();

        OneTileByOneButtons();
    }
}