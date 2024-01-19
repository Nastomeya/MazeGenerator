using System.Collections.Generic;
using UnityEngine;

public class MazeObjsController : TileBase
{
    internal List<IMazeObjects> MazeObjects { get; private set; }
    internal void InitObjectsIdentities(List<IMazeObjects> _objs)
    {
        MazeObjects = new List<IMazeObjects>();
        CreateObjects(_objs);
    }
    protected IMazeObjects FindWallWithID(int _id)
    {
        IMazeObjects newFoundWall = MazeObjects.Find(x => x.id == _id);
        return newFoundWall;
    }
    protected void CreateObjects(List<IMazeObjects> enabledWallList)
    {
        for (int i = 0; i < enabledWallList.Count; i++)
        {
            var wallHasFound = FindWallWithID(enabledWallList[i].id);
            if (wallHasFound == null)
                AddNewMazeObjectType(enabledWallList[i]);
            else
                wallHasFound.SetWallHolderEnable(maze.MazeObjectSetting.ObjectDatas[enabledWallList[i].id].ActiveSelf);
        }
    }
    protected void AddNewMazeObjectType(IMazeObjects _newObjects)
    {
        var newObject = CreateNewObjectFromPrefab(_newObjects);
        MazeObjects.Add(newObject);
    }
    private IMazeObjects CreateNewObjectFromPrefab(IMazeObjects _newMazeobjs)
    {
        var newObjectsTrans = Instantiate(_newMazeobjs.transform);
        newObjectsTrans.SetParent(transform);

        var newObjects = newObjectsTrans.GetComponent<IMazeObjects>();
        //if (newObjects.id == _newMazeobjs.id)
        //    Debug.Log("if you see this, remove SetID() in the following line : " + newObjects.id, context: transform);
        ////newObjects.SetID(_newMazeobjs.id);
        StartCoroutine(newObjects.ClearAllObjs());
        return newObjects;
    }
    protected void ChangeWallFormation(int _wallFormation, int _overlappingFormaption)
    {
        for (int i = 0; i < MazeObjects.Count; i++)
        {
            if (MazeObjects[i].transform.gameObject.activeSelf)
                if (MazeObjects[i].IsOverlappingMode)
                    MazeObjects[i].SetWallsEnable(_overlappingFormaption);
                else
                    MazeObjects[i].SetWallsEnable(_wallFormation);
        }
    }
}