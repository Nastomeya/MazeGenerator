using System.Collections.Generic;

public class MazeObjsController : BaseTile
{
    internal List<IMazeObjects> MazeObjects { get; private set; }
    internal void InitObjectsIdentities(List<IMazeObjects> _objs)
    {
        MazeObjects = new List<IMazeObjects>();
        CreateObjects(_objs);
    }
    protected IMazeObjects FindMazeObjectWithID(int _id)
    {
        IMazeObjects newObjFound = MazeObjects.Find(x => x.ID == _id);
        return newObjFound;
    }
    protected void CreateObjects(List<IMazeObjects> enabledMazeObjectList)
    {
        for (int i = 0; i < enabledMazeObjectList.Count; i++)
        {
            var mazeObjHasBeenFound = FindMazeObjectWithID(enabledMazeObjectList[i].ID);
            if (mazeObjHasBeenFound == null)
            {
                AddNewMazeObjectType(enabledMazeObjectList[i]);
                continue;
            }

            if (mazeObjHasBeenFound is BaseWall wall)
                wall.ActivateFunction(maze.MazeObjectSetting.ObjectDatas[enabledMazeObjectList[i].ID].ActiveSelf);
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

            var objFormation = MazeObjects[i] is IOverlapableFormation ? _overlappingFormation : _objectFormation;
            MazeObjects[i].SetMazeFormation(objFormation);
        }
    }
}