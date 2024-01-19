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
    protected IMazeObjects FindMazeObjectWithID(int _id)
    {
        IMazeObjects newObjFound = MazeObjects.Find(x => x.id == _id);
        return newObjFound;
    }
    protected void CreateObjects(List<IMazeObjects> enabledMazeObjectList)
    {
        for (int i = 0; i < enabledMazeObjectList.Count; i++)
        {
            var mazeObjHasBeenFound = FindMazeObjectWithID(enabledMazeObjectList[i].id);
            if (mazeObjHasBeenFound == null)
                AddNewMazeObjectType(enabledMazeObjectList[i]);
            else
                mazeObjHasBeenFound.SetHolderEnable(maze.MazeObjectSetting.ObjectDatas[enabledMazeObjectList[i].id].ActiveSelf);
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
        StartCoroutine(newObjects.ClearAllObjs());
        return newObjects;
    }
    protected void ChangeObjectFormation(int _objectFormation, int _overlappingFormation)
    {
        for (int i = 0; i < MazeObjects.Count; i++)
        {
            if (!MazeObjects[i].transform.gameObject.activeSelf)
                continue;
            var objFormation = MazeObjects[i].IsOverlappingMode ? _overlappingFormation : _objectFormation;


            MazeObjects[i].SetEnable(objFormation);
        }
    }
}