using UnityEngine;

public class TorchData : BaseObjectData
{
    public Transform transform;
    public TorchData(Transform _transform, int _position) : base(_position)
    {
        transform = _transform;
    }
}
