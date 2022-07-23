using MathCalculation;
using UnityEngine;

public class WallSizeAdjuster : MonoBehaviour
{
    internal void SetWallWidth(float _radius, float _degree)
    {
        //var currCollider = GetComponentInChildren<Collider>();
        //var thickness = AdvanceMath.GetColliderRadius(currCollider.transform, currCollider);

        var edgeSize = BasicMath.GetEdgeSize(_radius, _degree);
        transform.localScale = transform.localScale.With(x: edgeSize);
    }
    internal void SetWallWidth(float _radius)
    {
        transform.localScale = transform.localScale.With(x: _radius);
    }
}
