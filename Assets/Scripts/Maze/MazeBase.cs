using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RevealObjectController))]
public abstract class MazeBase : MonoBehaviour
{
    [SerializeField] protected int width, height;
    [SerializeField] private MazeObjectSettings mazeObjectSetting;
    [SerializeField] protected TileWithMazeObjectSets tileWithWallsPrefab;

    internal MazeObjectSettings MazeObjectSetting => mazeObjectSetting;

    public bool IsKeepTracking() => keepTracking;
    public void OnNextTile() => isNotFinished = false;
    public void OnLoopThrough() => loopThrough = true;
    public bool GetLoopThrough() => !loopThrough;
    public void ResetTiles()
    {
        InitTiles();
        SetupFristTile();
        BacktrackPath();
    }

    protected List<TileWithMazeObjectSets> tiles;
    protected Stack<TileWithMazeObjectSets> stack;
    protected TileWithMazeObjectSets currentTile;
    protected int totalTileNum, trackedTileNumber = 0;
    protected StraightPathController straightPathController;
    protected bool keepTracking;
    protected bool isShownInIndex = false;
    //protected List<IMazeObjects> allWalls;//remove this
    //private Dictionary<int, bool> currWallTypes;//directly update to List<IMazeWall> allWalls
    //internal Dictionary<int, bool> CurrWallTypes
    //{
    //    get
    //    {
    //        if (currWallTypes == null)
    //            currWallTypes = LoadData(true);
    //        return currWallTypes;
    //    }
    //}
    protected Vector2 offset;
    protected NeighboursBase currNeighbours;

    private void Start()
    {
        InitialMaze();
    }
    internal void InitialMaze()
    {
        //SetWallDataFromPrefabs();
        ResetAll();
        InitTiles();
        SetupFristTile();
        BacktrackPath();
    }
    private void ResetAll()
    {
        if (tiles == null || tiles.Count == 0)
            return;

        for (int i = 0; i < tiles.Count; i++)
        {
            Destroy(tiles[i].gameObject);
        }
        tiles.Clear();
    }
    //public Dictionary<int, bool> GetCurrWallTypes() => currWallTypes;
    public List<TileWithMazeObjectSets> GetTiles() => tiles;
    //internal List<Transform> GetAllWallPrefabs() => allObjectDatas.ToListOfMazeObjects();
    //public void SetWallDataFromPrefabs()
    //{
    //    allWalls = new List<IMazeObjects>();
    //    //currWallTypes?.Clear();
    //    //CurrWallTypes.ToString();// = LoadData(true);
    //}
    //public void UpdateWallTypes(Dictionary<int, bool> _newWallTypes) => currWallTypes = _newWallTypes;
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

    //protected List<IMazeObjects> MazeObjectAssignment()
    //{
    //    var currTileWallList = new List<IMazeObjects>();

    //    for (int i = 0; i < mazeObjectSetting.ObjectDatas.Count; i++)
    //    {
    //        if (mazeObjectSetting.ObjectDatas[i].ActiveSelf)
    //        {
    //            currTileWallList.Add(allWalls[i]);
    //        }

    //    }
    //    return MazeObjectSetting.ToListOfMazeObjects();
    //}

    protected void BacktrackPath() => StartCoroutine(BacktrackingPath());//while loop

    public void TirggerToChangeWallTypes(int _id, bool _isEnable)
    {
        if (tiles != null)
        {
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].EanbleMazeObjectSetIfExist(_id, _isEnable);
        }
    }
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
    //public abstract string GetFileNameAndPath();
    //public Dictionary<int, bool> LoadData(bool setupWallData = false)
    //{
    //    var dummyWallsIdentities = Utils.LoadDicFromFile(GetFilePath(), GetFileNameAndPath());

    //    if (dummyWallsIdentities == null || dummyWallsIdentities.Count < 1)
    //    {
    //        dummyWallsIdentities = new Dictionary<int, bool>();
    //        for (int i = 0; i < allObjectDatas.Count; i++)
    //        {
    //            dummyWallsIdentities.Add(i, false);
    //            if (setupWallData)
    //                InitWallData(i, allObjectDatas[i]);
    //        }

    //        Utils.SaveDicToFile(dummyWallsIdentities, GetFileNameAndPath());
    //    }
    //    else
    //    {
    //        if (setupWallData)
    //        {
    //            for (int i = 0; i < allObjectDatas.Count; i++)
    //                InitWallData(i, allObjectDatas[i]);
    //        }
    //    }
    //    return dummyWallsIdentities;
    //}
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
    //protected string GetFilePath() => Application.dataPath + "/mazeWallData";
    //private void InitWallData(int id, Transform mazeObjPrefab)
    //{
    //    var currWall = mazeObjPrefab.GetComponent<IMazeObjects>();
    //    currWall.SetID(id);
    //    allWalls.Add(currWall);
    //}
}