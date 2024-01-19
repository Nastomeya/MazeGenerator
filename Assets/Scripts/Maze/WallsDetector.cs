using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsDetector : MonoBehaviour
{
    private List<Renderer> allRenderers;
    private void Awake()
    {
        ResetWalls();
        StartCoroutine(SetEnable(false));
    }
    private void OnTriggerEnter(Collider other)
    {
        var wall = other.gameObject.GetComponent<Wall>();
        StartCoroutine(TriggerPillirs(wall));
    }
    private IEnumerator SetEnable(bool _isEnable)
    {
        for (int i = 0; i < allRenderers.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            allRenderers[i].enabled = _isEnable;
        }
    }

    private IEnumerator TriggerPillirs(Wall _wall)
    {
        if (_wall == null)
            yield break;
        if (_wall.PillirJobs == null)
            yield break;

        _wall.PillirJobs.Enqueue
        (
            () =>
            {
                if (gameObject.activeSelf)
                    SetEnable(false);//StartCoroutine(SetEnable(false));
            }
        );
        yield return new WaitForEndOfFrame();
        StartCoroutine(SetEnable(true));
    }
    internal void ResetWalls()
    {
        allRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
    }
}
