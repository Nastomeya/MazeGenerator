using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RevealObjectController))]
public abstract class MazeBase : MonoBehaviour
{
    [SerializeField] protected int width, height;
    [SerializeField] protected List<Transform> allWallPrefabs;
    [SerializeField] protected TileWithWalls tileWithWallsPrefab;
    public bool IsKeepTracking() => keepTracking;
    public void OnNextTile() => isNotFinished = false;
    public void OnLoopThrough() => loopThrough = true;
    public bool GetLoopThrough() => !loopThrough;

    protected List<TileWithWalls> tiles;
    protected Stack<TileWithWalls> stack;
    protected TileWithWalls currentTile;
    protected int totalTileNum, trackedTileNumber = 0;
    protected StraightPathController straightPathController;
    protected bool keepTracking;
    protected bool isShownInIndex = false;
    protected List<IMazeWalls> allWalls;
    protected Dictionary<int, bool> currWallTypes;//directly update to List<IMazeWall> allWalls
    protected Vector2 offset;
    protected NeighboursBase currNeighbours;

    private void Start()
    {
        SetWallDataFromPrefabs();
        InitTiles();

        SetupFristTile();
        BacktrackPath();
    }
    public Dictionary<int, bool> GetCurrWallTypes() => currWallTypes;
    public List<TileWithWalls> GetTiles() => tiles;
    public List<Transform> GetAllWallPrefabs() => allWallPrefabs;
    public void SetWallDataFromPrefabs()
    {
        allWalls = new List<IMazeWalls>();
        currWallTypes = LoadData(true);
    }
    public void UpdateWallTypes(Dictionary<int, bool> _newWallTypes) => currWallTypes = _newWallTypes;
    public IMazeWalls GetWallFromAll(int _id) => allWalls.Find(x => x.id == _id);
    private void InitTiles()
    {
        tiles = new List<TileWithWalls>();
        stack = new Stack<TileWithWalls>();
        totalTileNum = width * height - 1;
        offset = new Vector2((width - 1f) / 2f, (height - 1f) / 2f);

        CreateNewTiles();
    }
    protected void CreateNewTiles()
    {
        int index = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                CreateANewTile(ref index, y, x);
        }
    }
    protected abstract void CreateANewTile(ref int index, int y, int x);
    //protected abstract IEnumerator BacktrackingPath();
    [SerializeField] private Transform mark;
    private bool isNotFinished, loopThrough = false;
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
                    var formationId = currentTile.UpdateRemovalWallFormation(tiles[nextIndex]);//STEP 3
                    straightPathController.IdenticalRandomCheck(formationId);
                    currentTile = tiles[nextIndex];//STEP 4
                    break;
                }
                else
                    nextIndex = -1;
            }
            if (nextIndex < 0)
                currentTile.RemoveCurrWallFromList();
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
            currentTile.BacktrackRemovalWallFormation(nextTile);
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

    protected List<IMazeWalls> WallsAssignment()
    {
        var currTileWallList = new List<IMazeWalls>();
        for (int i = 0; i < allWalls.Count; i++)
        {
            var hasFoundValue = currWallTypes.TryGetValue(allWalls[i].id, out bool isEnable);
            if (hasFoundValue)
            {
                if (isEnable)
                    currTileWallList.Add(allWalls[i]);
            }
        }
        return currTileWallList;
    }

    protected void BacktrackPath() => StartCoroutine(BacktrackingPath());//while loop

    public void TirggerToChangeWallTypes(int _id, bool _isEnable)
    {
        if (tiles != null)
        {
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].EanbleWallIfExist(_id, _isEnable);
        }
    }
    protected void SetVisited(TileWithWalls _tile)
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
    public abstract string GetFileNameAndPath();
    public Dictionary<int, bool> LoadData(bool setupWallData = false)
    {
        var dummyWallsIdentities = Utils.LoadDicFromFile(GetFilePath(), GetFileNameAndPath());

        if (dummyWallsIdentities == null || dummyWallsIdentities.Count < 1)
        {
            dummyWallsIdentities = new Dictionary<int, bool>();
            for (int i = 0; i < allWallPrefabs.Count; i++)
            {
                dummyWallsIdentities.Add(i, false);
                if (setupWallData)
                    InitWallData(i, allWallPrefabs[i]);
            }

            Utils.SaveDicToFile(dummyWallsIdentities, GetFileNameAndPath());
        }
        else
        {
            if (setupWallData)
            {
                for (int i = 0; i < allWallPrefabs.Count; i++)
                    InitWallData(i, allWallPrefabs[i]);
            }
        }
        return dummyWallsIdentities;
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
        for (int i = 0; i < tiles.Count; i++)
            tiles[i].ResetTile(isShownInIndex);
    }
    protected string GetFilePath() => Application.dataPath + "/mazeWallData";
    private void InitWallData(int id, Transform wallPrefab)
    {
        var currWall = wallPrefab.GetComponent<IMazeWalls>();
        currWall.SetID(id);
        allWalls.Add(currWall);
    }
}