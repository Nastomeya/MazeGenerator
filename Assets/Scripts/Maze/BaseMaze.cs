using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RevealObjectController))]
public abstract class BaseMaze : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] protected TileWithMazeObjectSets tileWithWallsPrefab;
    [SerializeField] private MazeObjectSettings mazeObjectSetting;
    [SerializeField] private Transform mark;
    internal int Width => width;
    internal int Height => height;
    internal MazeObjectSettings MazeObjectSetting => mazeObjectSetting;

    private bool isNotFinished, loopThrough = false;
    internal bool IsFinished => !isNotFinished;

    protected List<TileWithMazeObjectSets> tiles;
    protected Stack<TileWithMazeObjectSets> stack;
    protected TileWithMazeObjectSets currentTile;
    protected int totalTileNum, trackedTileNumber = 0;
    protected StraightPathController straightPathController;
    protected bool keepTracking;
    protected bool isShownInIndex = false;
    protected Vector2 offset;
    protected BaseNeighbours currNeighbours;

    private void Start() => InitialMaze();
    internal void InitialMaze()
    {
        SetWallDataFromPrefabs();
        OnButtonToResetTiles();
        DestoryTiles();
        InitTiles();
        SetupFristTile();
        BacktrackPath();
    }

    protected abstract void CreateANewTile(ref int index, int y, int x);
    public bool IsKeepTracking() => keepTracking;
    public void OnNextTile() => isNotFinished = false;
    public void OnLoopThrough() => loopThrough = true;
    public bool GetLoopThrough() => !loopThrough;

    private void DestoryTiles()//should be using the pool technique instead.
    {
        if (tiles == null || tiles.Count == 0)
            return;

        for (int i = 0; i < tiles.Count; i++)
        {
            Destroy(tiles[i].gameObject);
        }
        tiles.Clear();
    }
    public List<TileWithMazeObjectSets> GetTiles() => tiles;
    public void SetWallDataFromPrefabs()
    {
        for (int i = 0; i < MazeObjectSetting.ObjectDatas.Count; i++)
        {
            MazeObjectSetting.ObjectDatas[i].MazeObjects.SetID(i);
        }
    }
    public IMazeObjects GetMazeObjectSetlFromSetting(int _id)
    {
        if (MazeObjectSetting.ObjectDatas[_id] != null)
            return MazeObjectSetting.ObjectDatas[_id].MazeObjects;
        return null;
    }
    private void InitTiles()
    {
        tiles = new List<TileWithMazeObjectSets>();
        stack = new Stack<TileWithMazeObjectSets>();
        totalTileNum = Width * Height - 1;
        offset = new Vector2((Width - 1f) / 2f, (Height - 1f) / 2f);

        CreateNewTiles();
    }
    protected void CreateNewTiles()
    {
        int index = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                CreateANewTile(ref index, y, x);
            }
        }
    }

    protected virtual IEnumerator BacktrackingPath()
    {
        isNotFinished = true;
        keepTracking = true;
        int nextIndex = -1;

        SetMarkPosition(currentTile.transform.position);

        while (currentTile.Turns >= 0)
        {
            nextIndex = currentTile.GetNeighbourIndex(straightPathController);//STEP 1// return back to -1 if it gets nothing in return
            if (nextIndex >= 0)
            {
                if (!tiles[nextIndex].isVisited)
                {
                    stack.Push(currentTile);//STEP 2
                    var formationId = currentTile.UpdateRemovalObjectFormation(tiles[nextIndex]);//STEP 3
                    straightPathController.IdenticalRandomCheck(formationId);
                    currentTile = tiles[nextIndex];//STEP 4
                    break;
                }
                else
                    nextIndex = -1;
            }
            if (nextIndex < 0)
                currentTile.RemoveCurrObjectFromList();
        }

        keepTracking = false;
        if (loopThrough)
            isNotFinished = false;

        yield return new WaitWhile(() => isNotFinished);

        if (nextIndex >= 0)
        {
            SetVisited(currentTile);
            straightPathController.SetNextIndex(nextIndex);
            BacktrackPath();
        }
        else if (stack.Count > 0)
        {
            var nextTile = stack.Pop();
            currentTile.BacktrackRemovalObjFormation(nextTile);
            currentTile = nextTile;

            if (trackedTileNumber > totalTileNum)//Stop tracking if it is the last tile.
            {
                loopThrough = false;
                yield break;
            }
            BacktrackPath();
        }
    }
    private void SetMarkPosition(Vector3 targetPosition) => mark.transform.position = targetPosition.With(y: mark.transform.position.y);
    protected void BacktrackPath() => StartCoroutine(BacktrackingPath());//while loop

    protected void SetVisited(TileWithMazeObjectSets _tile)
    {
        if (!_tile.isVisited)
        {
            if (isShownInIndex)
                _tile.SetCurrentTileVisited();
            else
                _tile.SetCurrentTileVisited(trackedTileNumber);

            trackedTileNumber++;
        }
    }
    public void OnButtonToCreateNewMaze(bool _isShownInIndex)
    {
        isShownInIndex = _isShownInIndex;
        OnButtonToResetTiles();
        SetupFristTile();
        BacktrackPath();
    }
    private void SetupFristTile()
    {
        var initialIndex = Utils.RandomRange(0, totalTileNum);
        currentTile = tiles[initialIndex];

        straightPathController = new StraightPathController();
        trackedTileNumber = 0;
        SetVisited(currentTile);
        stack.Clear();
    }
    private void OnButtonToResetTiles()
    {
        if (tiles == null)
            return;

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].ResetTile(isShownInIndex);
        }
    }
}