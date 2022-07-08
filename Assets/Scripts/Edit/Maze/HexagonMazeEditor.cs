using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexagonMaze))]
public class HexagonMazeEditor : MazeBaseEditor
{
    protected void Awake()
    {
        maze = (HexagonMaze)target;
        Init();
    }
    protected override void OneTileByOneButtons()
    {
        if (!((HexagonMaze)maze).IsKeepTracking())
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Next Tile", GUILayout.Height(50)))
                ((HexagonMaze)maze).OnNextTile();//remove prefab

            GUI.backgroundColor = Color.red;
            GUI.enabled = ((HexagonMaze)maze).GetLoopThrough();
            if (GUILayout.Button("Loop Through All Tiles", GUILayout.Height(50)))
                ((HexagonMaze)maze).OnLoopThrough();
        }
    }
}