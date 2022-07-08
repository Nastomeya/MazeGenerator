using DataExtensions;
using System;
using System.Collections.Generic;

[Serializable]
public class IdentityCollector
{
    public Dictionary<int, bool> identities;

    public IdentityCollector()
    {
        identities = DataRenewer.Renew(identities);
    }
    //public void ReplaceIdentities(List<IMazeWall> mazeWalls)//why do I need this?
    //{
    //    mazeWalls = DataRenewer.Renew(mazeWalls);

    //    for (int i = 0; i < mazeWalls.Count; i++)
    //        identities.Add(mazeWalls[i].id, false);
    //}

}