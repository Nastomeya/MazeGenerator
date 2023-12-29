using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RevealObjectController))]
public class SingleTileInDebugMode : MonoBehaviour
{
    [SerializeField] internal List<Transform> allWallPrefabs;//Don't change it back to readonly
    [SerializeField] private TileShapes tileShapes;
    [SerializeField] private TileWithWalls tileWithWallsPrefab;//Don't change it back to readonly

    private List<IMazeWalls> allWalls;
    private NeighboursBase neighbours;

    private void Awake()
    {
        SetWallDataFromPrefabs();
        tileWithWallsPrefab.InitWallIdentities(allWalls);
    }
    private void Start() => tileWithWallsPrefab.trackText.Init();

    public int GetWallFormation() => neighbours.GetWallFormation();
    public void InitTile()
    {
        if (tileShapes == TileShapes.Square)
            neighbours = new SquareNeighbours(0, 0, 0, null);
        else
            neighbours = new HexagonNeighbours(0, 0, 0, null);
        if (neighbours != null)
        {
            neighbours.InitFormation();
            EnableTheWalls();
        }
    }
    public void SetWallDataFromPrefabs()
    {
        allWalls = new List<IMazeWalls>();
        for (int i = 0; i < allWallPrefabs.Count; i++)
        {
            var currWall = allWallPrefabs[i].GetComponent<IMazeWalls>();
            currWall.SetID(i);
            allWalls.Add(currWall);
        }
    }
    public List<IMazeWalls> GetWallTypes() => allWalls;

    public void ChangeWallFormation(int _currFormation = 0)
    {
        var isWallExist = CheckTileExistence();
        Debug.Log("isWallExist  " + isWallExist);
        if (isWallExist)
        {
            if (_currFormation > 0)
                neighbours.SetWallFormation(_currFormation);
            else
                neighbours.WallIncrement();
            EnableTheWalls();
        }
    }
    public void EnableTheWall(int _wallIndex, bool _isEnable)
    {
        if (CheckTileExistence())
        {
            var currWallFormation = neighbours.GetWallFormation();
            int _wallFormation = (!_isEnable) ? 0 : currWallFormation;

            tileWithWallsPrefab.Walls[_wallIndex].SetWallHolderEnable(_isEnable);
            tileWithWallsPrefab.Walls[_wallIndex].SetWallsEnable(_wallFormation);
        }
    }
    private void EnableTheWall(int _wallIndex)
    {
        if (CheckTileExistence())
        {
            int currWallFormation = neighbours.GetWallFormation();
            var _isEnable = tileWithWallsPrefab.Walls[_wallIndex].transform.gameObject.activeSelf;
            if (_isEnable)
                tileWithWallsPrefab.Walls[_wallIndex].SetWallsEnable(currWallFormation);
        }
    }
    private void EnableTheWalls()
    {
        if (tileWithWallsPrefab.Walls != null)
            for (int i = 0; i < tileWithWallsPrefab.Walls.Count; i++)
                EnableTheWall(i);
    }
    private bool CheckTileExistence() => tileWithWallsPrefab.Walls != null && tileWithWallsPrefab.Walls.Count > 0;
}
public enum TileShapes
{
    Square,
    Hexagon
}