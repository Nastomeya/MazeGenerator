using System.Collections;
using UnityEngine;
public interface IMazeWalls
{
    public bool InOverlappingMode { get; }//remove this later
    public int id { get; }
    public Transform transform { get; }
    public void SetID(int _newID);
    public void SetWallHolderEnable(bool _isEnable);
    public void SetWallsEnable(int _wallFormation);
    public IEnumerator ClearAllWalls();
}