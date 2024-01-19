using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableData/Maze/ViewControlSetting", fileName = "ViewControlSetting")]
public class MazeObjectSettings : ScriptableObject
{
    [SerializeField] private List<MazeObjsSetData> objectDatas;
    internal List<MazeObjsSetData> ObjectDatas => objectDatas;

    internal List<IMazeObjects> ToListOfMazeObjects()
    {
        var listOfObjects = new List<IMazeObjects>();
        for (int i = 0; i < objectDatas.Count; i++)
        {
            var newMazeObjects = objectDatas[i].MazeObjects;
            if (newMazeObjects == null)
                continue;
            if (ObjectDatas[i].ActiveSelf)
                listOfObjects.Add(newMazeObjects);
        }
        return listOfObjects;
    }
    internal IMazeObjects Find(int _id)
    {
        return objectDatas[_id].MazeObjects;
    }
}
