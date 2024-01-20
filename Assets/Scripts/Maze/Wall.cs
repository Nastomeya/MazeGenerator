using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IMazeElement
{
    internal List<Action> DoJobsWhenDisable = new();

    private void OnDisable()
    {
        if (DoJobsWhenDisable == null)
            return;

        foreach (var doJob in DoJobsWhenDisable)
        {
            doJob();
        }

        DoJobsWhenDisable.Clear();
    }
}