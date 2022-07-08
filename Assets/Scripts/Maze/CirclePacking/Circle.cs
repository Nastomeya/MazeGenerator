using UnityEngine;

public class Circle : ObjectBase
{
    private float radius;

    internal override void SetSize(float _radius)
    {
        radius = _radius;
        transform.localScale = new Vector3(_radius * 2f, transform.localScale.y, _radius * 2f);
    }
    internal override float GetSize() => radius;
}