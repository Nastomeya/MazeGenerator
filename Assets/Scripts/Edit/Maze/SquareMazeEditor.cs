using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SquareMaze))]
public class SquareMazeEditor : MazeBaseEditor
{
    protected void Awake()
    {
        maze = (SquareMaze)target;
        Init();
    }
    protected override void OneTileByOneButtons()
    {
        if (!((SquareMaze)maze).IsKeepTracking())
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Next Tile", GUILayout.Height(50)))
                ((SquareMaze)maze).OnNextTile();//remove prefab

            GUI.backgroundColor = Color.red;
            GUI.enabled = ((SquareMaze)maze).GetLoopThrough();
            if (GUILayout.Button("Loop Through All Tiles", GUILayout.Height(50)))
                ((SquareMaze)maze).OnLoopThrough();
        }
    }
}
