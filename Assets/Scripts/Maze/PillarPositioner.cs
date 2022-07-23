using MathCalculation;
using UnityEngine;

public class PillarPositioner : MonoBehaviour
{
    [SerializeField] private Collider currCollider;
    private float ratio;
    private void Awake()
    {
        currCollider = GetComponent<Collider>();
    }
    internal void SetPosition(float _radius, float _angleInDegrees, Vector2 _origin)
    {
        ratio = _radius / 0.6f;
        var selfRadius = AdvanceMath.GetColliderRadius(transform, currCollider) * 0.9f * ratio;
        var newPosi = AdvanceMath.PointInCircumference(_radius + selfRadius, _angleInDegrees, _origin);
        transform.position = new Vector3(newPosi.x, transform.position.y, newPosi.y);
    }
}
