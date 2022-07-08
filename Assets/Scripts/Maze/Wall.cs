using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    internal Queue<Action> PillirJobs;
    private void Awake()
    {
        PillirJobs = new Queue<Action>();
    }
    private void OnDisable()
    {
        if (PillirJobs != null)
        {
            foreach (var doJob in PillirJobs)
            {
                doJob();
            }
            if (PillirJobs.Count > 0)
                PillirJobs.Dequeue();
        }
    }
}
