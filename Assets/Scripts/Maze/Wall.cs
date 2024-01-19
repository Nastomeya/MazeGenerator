using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IMazeElement
{
    internal Queue<Action> PillirJobs;
    private void Awake()
    {
        PillirJobs = new Queue<Action>();
    }
    private void OnDisable()
    {
        if (PillirJobs == null)
            return;

        foreach (var doJob in PillirJobs)
        {
            doJob();
        }
        if (PillirJobs.Count > 0)
            PillirJobs.Dequeue();
        //PillirJobs.Clear();
    }
}
