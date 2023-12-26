using MathCalculation;
using UnityEngine;

public class WallSizeAdjuster : MonoBehaviour
{
    internal void SetWallWidth(float _radius, float _degree)
    {
        //var currCollider = GetComponentInChildren<Collider>();
        //var thickness = AdvanceMath.GetColliderRadius(currCollider.transform, currCollider);

        var division = (360f / _degree);
        var edge = BasicMath.GetEdgeSize(_radius, _degree);
        var pillarWidth = _radius * Mathf.Cos(Mathf.Deg2Rad * _degree);

        var width = edge + ((edge + pillarWidth) / division);
        transform.localScale = transform.localScale.With(x: width);
    }

    internal void SetWallWidth(float _radius)
    {
        transform.localScale = transform.localScale.With(x: _radius);
    }
}
