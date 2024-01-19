using System.Collections;
using UnityEngine;
/// <summary>It could be the set of walls, torches, rocks, etc. that can fit in a maze tile. <see cref="IMazeElement"/>
///<br/>     ***** 
///<br/>    *  X  *
///<br/>   *X     X*
///<br/>  *         *
///<br/>   *X     X*
///<br/>    *  X  *
///<br/>     *****
///<br/> The Xs are the <see cref="IMazeObjects"/> which is combination of <see cref="IMazeElement"/></summary>
[SerializeField]
public interface IMazeObjects
{
    public bool IsOverlappingMode { get; }//remove this later
    public int id { get; }
    public Transform transform { get; }
    public void SetID(int _newID);
    public void SetHolderEnable(bool _isEnable);
    public void SetEnable(int _objectFormation);
    public IEnumerator ClearAllObjs();
}