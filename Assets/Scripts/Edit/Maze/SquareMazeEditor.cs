using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SquareMaze))]
public class SquareMazeEditor : BaseMazeEditor
{
    protected void Awake()
    {
        if (!Application.isPlaying)
            return;

        maze = (SquareMaze)target;
        ObjectRawDatas.ToString();
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
