#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexagonMaze))]
public class HexagonMazeEditor : BaseMazeEditor
{
    protected void Awake()
    {
        if (!Application.isPlaying)
            return;

        maze = (HexagonMaze)target;
        ObjectRawDatas.ToString();
    }
    protected override void OneTileByOneButtons()
    {
        if (maze is not HexagonMaze hexagon)
            return;
        if (hexagon.IsKeepTracking())
            return;

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Next Tile", GUILayout.Height(50)))
            hexagon.OnNextTile();//remove prefab

        GUI.backgroundColor = Color.red;
        GUI.enabled = hexagon.GetLoopThrough();
        if (GUILayout.Button("Loop Through All Tiles", GUILayout.Height(50)))
            hexagon.OnLoopThrough();

        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Reset", GUILayout.Height(50)))
            hexagon.InitialMaze();
    }
}
#endif