using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RevealObjectController))]
public class SingleTileInDebugMode : MonoBehaviour
{
    [SerializeField] internal List<Transform> allMazeObjPrefabs;//Don't change it back to readonly
    [SerializeField] private TileShapes tileShapes;
    [SerializeField] private TileWithMazeObjectSets tileWithMazeObjectSetsPrefab;//Don't change it back to readonly

    private List<IMazeObjects> allObjects;
    public List<IMazeObjects> MazeObjects => allObjects;

    private BaseNeighbours neighbours;

    private void Awake()
    {
        InitMazeDataFromPrefabs();
        tileWithMazeObjectSetsPrefab.InitObjectsIdentities(allObjects);
    }
    private void Start() => tileWithMazeObjectSetsPrefab.trackText.Init();

    public int GetMazeFormation() => neighbours.GetMazeFormation();
    public void InitTile()
    {
        neighbours = tileShapes == TileShapes.Square ? new SquareNeighbours(0, 0, 0, null) : new HexagonNeighbours(0, 0, 0, null);

        if (neighbours == null)
            return;

        neighbours.InitFormation();
        EnableTheMazeObjs();
    }
    private void InitMazeDataFromPrefabs()
    {
        allObjects = new List<IMazeObjects>();

        Debug.Log("InitMazeDataFromPrefabs " + allMazeObjPrefabs.Count);


        for (int i = 0; i < allMazeObjPrefabs.Count; i++)
        {
            var currObject = allMazeObjPrefabs[i].GetComponent<IMazeObjects>();
            currObject.SetID(i);
            allObjects.Add(currObject);
        }
    }

    public void ChangeMazeFormation(int _currFormation = 0)
    {
        var isObjectExist = IsTheMazeObjPrefabExist();
        if (!isObjectExist)
            return;

        if (_currFormation > 0)
            neighbours.SetMazeFormation(_currFormation);
        else
            neighbours.Increment();
        EnableTheMazeObjs();
    }
    public void EnableTheMazeObject(int _objIndex, bool _isEnable)
    {
        if (!IsTheMazeObjPrefabExist(_objIndex))
            return;

        var currMazeFormation = neighbours.GetMazeFormation();
        int _mazeFormation = (!_isEnable) ? 0 : currMazeFormation;

        if (tileWithMazeObjectSetsPrefab.MazeObjects[_objIndex] is BaseWall wall)
            wall.ActivateFunction(_isEnable);
        tileWithMazeObjectSetsPrefab.MazeObjects[_objIndex].SetEnble(_isEnable);
        tileWithMazeObjectSetsPrefab.MazeObjects[_objIndex].SetMazeFormation(_mazeFormation);
    }
    private void EnableTheMazeObject(int _objIndex)
    {
        if (!IsTheMazeObjPrefabExist(_objIndex))
            return;

        int currMazeFormation = neighbours.GetMazeFormation();
        var _isEnable = tileWithMazeObjectSetsPrefab.MazeObjects[_objIndex].transform.gameObject.activeSelf;
        if (_isEnable)
            tileWithMazeObjectSetsPrefab.MazeObjects[_objIndex].SetMazeFormation(currMazeFormation);
    }
    private void EnableTheMazeObjs()
    {
        if (tileWithMazeObjectSetsPrefab.MazeObjects == null)
            return;

        for (int i = 0; i < tileWithMazeObjectSetsPrefab.MazeObjects.Count; i++)
        {
            EnableTheMazeObject(i);
        }
    }
    private bool IsTheMazeObjPrefabExist(int _index)
    {
        if (!IsTheMazeObjPrefabExist())
            return false;
        if (_index < 0)
            return false;
        if (_index > (tileWithMazeObjectSetsPrefab.MazeObjects.Count - 1))
            return false;
        if (tileWithMazeObjectSetsPrefab.MazeObjects[_index] == null)
            return false;

        return true;
    }
    private bool IsTheMazeObjPrefabExist()
    {
        if (tileWithMazeObjectSetsPrefab.MazeObjects == null)
            return false;
        if (tileWithMazeObjectSetsPrefab.MazeObjects.Count < 1)
            return false;

        return true;
    }
}
public enum TileShapes
{
    Square,
    Hexagon
}