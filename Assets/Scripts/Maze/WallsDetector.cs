using System.Collections.Generic;
using UnityEngine;

public class WallsDetector : MonoBehaviour
{
    private List<Renderer> allRenderers;
    private void Awake()
    {
        allRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
        SetEnable(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        var wall = other.gameObject.GetComponent<Wall>();

        if (wall != null && wall.PillirJobs != null)
        {
            wall.PillirJobs.Enqueue(() => SetEnable(false));
            SetEnable(true);
        }
    }
    private void SetEnable(bool _isEnable)
    {
        for (int i = 0; i < allRenderers.Count; i++)
        {
            allRenderers[i].enabled = _isEnable;
        }
    }
}
