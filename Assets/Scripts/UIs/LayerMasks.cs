using UnityEngine;

public class LayerMasks
{
    internal int MyLayerMask { get; private set; }
    internal int EnemyLayerMask { get; private set; }
    internal static int ObstacleLayerMask { get { return LayerMask.GetMask("Obstacle"); } }//DO NOT USE STATIC virables ON UPDATE() method
    internal static int GroundLayerMask { get { return LayerMask.GetMask("Ground"); } }
    internal static int DefaultLayerMask { get { return LayerMask.GetMask("Default"); } }
    internal static int MarkerLayerMask { get { return LayerMask.GetMask("Marker"); } }
    internal static int FireBallMask { get { return LayerMask.GetMask("Fireball"); } }
    internal static int Wall { get { return LayerMask.GetMask("Wall"); } }
    internal static int MyBuilding { get { return LayerMask.GetMask("TeamABuilding"); } }
    internal static int UI { get { return LayerMask.GetMask("UI"); } }

    public LayerMasks(int _layer)
    {
        MyLayerMask = LayerToMask(_layer);
        var teamA = LayerMask.GetMask("TeamA");
        var teamB = LayerMask.GetMask("TeamB");
        EnemyLayerMask = ((MyLayerMask & teamA) > 0) ? teamB : teamA;
    }
    internal static bool IsIdenticalMask(int _layerA, int _maskB)
    {
        return (LayerToMask(_layerA) & _maskB) > 0;//return true if different
    }
    internal static int LayerToMask(int _layer)//bitewise binary
    {
        return 1 << _layer;
    }

    internal static int MaskToLayer(int _mask)
    {
        return (int)Mathf.Log(_mask, 2);
    }
    internal static bool CheckIfTeamA(int gameObjectLayer)
    {
        bool isTeamA = (LayerToMask(gameObjectLayer) & LayerMask.GetMask("TeamA")) > 0;
        return isTeamA;
    }
}
