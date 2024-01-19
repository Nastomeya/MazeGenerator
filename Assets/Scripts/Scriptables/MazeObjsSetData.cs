using System;
using UnityEngine;

[Serializable]
public class MazeObjsSetData
{
    [SerializeField] private Transform mazeObjTrans;
    [SerializeField] private bool activeSelf;
    private IMazeObjects mazeObjects = null;
    internal IMazeObjects MazeObjects
    {
        get
        {
            if (mazeObjects == null)
                mazeObjects = mazeObjTrans.GetComponent<IMazeObjects>();
            return mazeObjects;
        }
    }

    internal bool ActiveSelf => activeSelf;
    internal void SetActive(bool _value)
    {
        activeSelf = _value;
    }
}