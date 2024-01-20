using System.Collections.Generic;
using UnityEngine;

public abstract class WallsDetector : MonoBehaviour
{
    private List<Renderer> allRenderers;
    protected abstract int WallMaxCount { get; }
    private static int currWallCount = 0;
    private void Awake()
    {
        ResetWalls();
        SetEnable(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        var wall = other.gameObject.GetComponent<Wall>();
        if (wall == null)
            return;

        if (currWallCount > WallMaxCount)
            currWallCount = WallMaxCount;
        else
            currWallCount++;

        SetEnable(true);
        AssignJobsForWall(wall);
    }
    private void SetEnable(bool _isEnable)
    {
        for (int i = 0; i < allRenderers.Count; i++)
        {
            if (allRenderers[i] == null)
                continue;
            allRenderers[i].enabled = _isEnable;
        }
    }
    private void AssignJobsForWall(Wall _wall)
    {
        _wall.DoJobsWhenDisable.Add
        (
            () => SetDisable(ref currWallCount)
        );
    }
    private void SetDisable(ref int _currWallCount)
    {
        _currWallCount--;
        if (_currWallCount > 0)
            return;

        _currWallCount = 0;
        SetEnable(false);
    }

    internal void ResetWalls()
    {
        allRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
        currWallCount = 0;
    }
}
