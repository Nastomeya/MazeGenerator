using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private Button createEntireMazeBtn;
    [SerializeField] private Button createNextTileBtn;
    [SerializeField] private Slider mazeWidthSlider;
    [SerializeField] private Slider mazeHeightSlider;
    private BaseMaze maze;
    private int lastWidth, lastHeight;

    private bool HasMazeSizeBeenChanged => (lastWidth != CurrentWidth) || (lastHeight != CurrentHeight);
    private int CurrentWidth => Mathf.RoundToInt(mazeWidthSlider.value);
    private int CurrentHeight => Mathf.RoundToInt(mazeHeightSlider.value);
    private void Awake()
    {
        maze = GetComponent<BaseMaze>();
        if (maze != null && HasMazeSizeBeenChanged)
            UpdateMazeSize();

        mazeWidthSlider.maxValue = 10;
        mazeWidthSlider.minValue = 2;
        mazeHeightSlider.maxValue = 10;
        mazeHeightSlider.minValue = 1;

        mazeWidthSlider.value = maze.Width;
        mazeHeightSlider.value = maze.Height;

        var widthSliderDisplay = mazeWidthSlider.GetComponent<SliderValueDisplay>();
        var heightSliderDisplay = mazeHeightSlider.GetComponent<SliderValueDisplay>();

        widthSliderDisplay.UpdateDisplayValue(maze.Width);
        heightSliderDisplay.UpdateDisplayValue(maze.Height);

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
        lastWidth = CurrentWidth;
        lastHeight = CurrentHeight;
        maze.SetMazeSize(CurrentWidth, CurrentHeight);
    }
}
