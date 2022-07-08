using UnityEngine;

public class RockObject : ObjectBase
{
    [SerializeField] private float radius;

    internal override void SetSize(float _size)
    {
        transform.localScale = new Vector3(_size * 1.1f, _size * 1.1f, _size * 1.1f);        
    }
    internal override float GetSize() => radius;

}
