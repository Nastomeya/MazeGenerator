using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private Button createEntireMazeBtn;
    [SerializeField] private Button createNextTileBtn;
    private BaseMaze maze;
    private int lastWidth, lastHeight;
    private bool HasMazeSizeBeenChanged => (lastWidth != maze.Width) || (lastHeight != maze.Height);

    private void Awake()
    {
        maze = GetComponent<BaseMaze>();
        if (maze != null && HasMazeSizeBeenChanged)
            UpdateMazeSize();

        createEntireMazeBtn.onClick.AddListener(GenerateTheEntireMaze);
        createNextTileBtn.onClick.AddListener(GenerateOneAtATime);
    }
    private void OnDestroy()
    {
        createEntireMazeBtn.onClick.RemoveListener(GenerateTheEntireMaze);
        createNextTileBtn.onClick.RemoveListener(GenerateOneAtATime);
    }
    private void GenerateOneAtATime()
    {
        if (maze.IsFinished)
        {
            if (HasMazeSizeBeenChanged)
            {
                UpdateMazeSize();
                maze.InitialMaze();
            }
            else
                maze.OnButtonToCreateNewMaze(false);
            return;
        }
        maze.OnNextTile();
    }
    private void GenerateTheEntireMaze()
    {
        maze.OnLoopThrough();
        GenerateOneAtATime();
    }
    private void UpdateMazeSize()
    {
        lastWidth = maze.Width;
        lastHeight = maze.Height;
    }
}
