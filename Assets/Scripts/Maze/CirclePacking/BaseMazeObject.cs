using System.Collections;
using UnityEngine;

public abstract class BaseMazeObject : MonoBehaviour
{
    public abstract IEnumerator ClearAllObjs();
    public int ID { get; private set; }
    public void SetID(int _newID) => ID = _newID;
}