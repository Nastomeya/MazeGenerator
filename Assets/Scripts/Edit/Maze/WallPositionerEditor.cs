using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallPositioner))]
public class WallPositionerEditor : Editor
{
    private WallPositioner wallPositioner;
    private Transform newWall = null;

    private void Awake()
    {
        wallPositioner = (WallPositioner)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var isPlaceWallBtnClicked = GUILayout.Button("PlaceWall", GUILayout.Height(50));
        if (isPlaceWallBtnClicked)
            wallPositioner.PlaceWalls();

        //var isGenerateAllBtnClicked = GUILayout.Button("GenerateAll", GUILayout.Height(50));
        //if (isGenerateAllBtnClicked)
        //    wallPositioner.GenerateAllWalls();
    }
}