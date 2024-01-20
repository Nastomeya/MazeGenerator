using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CirclePackPositioningNotifier : MonoBehaviour
{
    internal Action<List<UnitPositionOnWall>> OnTileAvaliable = delegate { };
    private BaseCirclePacking circlePacking;
    private List<UnitPositionOnWall> currAvaliablePostions;
    internal List<UnitPositionOnWall> GetPositions() => currAvaliablePostions;

    private void Awake()
    {
        circlePacking = GetComponent<BaseCirclePacking>();
        currAvaliablePostions = new List<UnitPositionOnWall>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var position = other.GetComponent<UnitPositionOnWall>();//this might be the issue for performance later.
        if (position != null)
        {
            if (!currAvaliablePostions.Contains(position))
            {
                currAvaliablePostions.Add(position);
                int lastAddedIndex = currAvaliablePostions.IndexOf(position);
                currAvaliablePostions[lastAddedIndex].OnPositionOccupied += Notify;
                currAvaliablePostions[lastAddedIndex].OnPositionRemove += RemovePosition;
            }
            Notify();
        }
    }
    private void RemovePosition(UnitPositionOnWall unitPosition)
    {
        if (currAvaliablePostions.Contains(unitPosition))
        {
            var _index = currAvaliablePostions.IndexOf(unitPosition);
            currAvaliablePostions[_index].OnPositionOccupied -= Notify;
            currAvaliablePostions[_index].OnPositionRemove -= RemovePosition;
            //Debug.Log(currAvaliablePostions.Count + "   removing!:  " + unitPosition);
            currAvaliablePostions.Remove(unitPosition);
            Notify();
        }
        //else
        //{
        //    Debug.Log("Not contain!  " + unitPosition);
        //    Debug.Log("But it contains...!  ");
        //    for (int i = 0; i < currAvaliablePostions.Count; i++)
        //    {
        //        Debug.Log(currAvaliablePostions[i]);

        //    }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        var position = other.GetComponent<UnitPositionOnWall>();
        if (position != null)
        {
            Debug.Log("<color=green>Bye   !!</color>  " + other.name);
            RemovePosition(position);
        }
    }
    private void OnDisable()
    {
        if (currAvaliablePostions != null)
        {
            for (int i = 0; i < currAvaliablePostions.Count; i++)
            {
                currAvaliablePostions[i].OnPositionOccupied -= Notify;
                currAvaliablePostions[i].OnPositionRemove -= RemovePosition;
            }
        }
    }
    private void Notify()
    {
        OnTileAvaliable?.Invoke(currAvaliablePostions);
    }

    #region UI Bottn, remove it later
    private Rect rect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100);
    void OnGUI()
    {
        if (GUI.Button(rect, "Reset Dummy?"))
            ResetDummy();
    }

    private bool isAvaliable = false;
    private void ResetDummy()
    {
        isAvaliable = !isAvaliable;
        OnTileAvaliable?.Invoke(null);
    }
    #endregion
}
