using System;
using UnityEngine;

public class UnitPositionOnWall : ObjectBase
{
    [SerializeField] private bool isOccupied = false;
    private bool lastOccupied = false;
    private string currOccupyUnit;
    public bool IsOccupied { get { return isOccupied; } }
    public string CurrOccupyUnit { get { return currOccupyUnit; } }

    private float radius;
    internal Action OnPositionOccupied = delegate { };
    internal Action<UnitPositionOnWall> OnPositionRemove = delegate { };
    private int gourndLayer;
    protected override void Awake()
    {
        base.Awake();
        gourndLayer = LayerMasks.MaskToLayer(LayerMasks.GroundLayerMask);
    }
    internal override void SetSize(float _radius)
    {
        radius = _radius;
        transform.localScale = new Vector3(_radius * 2f, transform.localScale.y, _radius * 2f);
    }
    internal override float GetSize() => radius;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != gourndLayer)
        {
            currOccupyUnit = other.name;
            isOccupied = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != gourndLayer)
        {
            isOccupied = false;
            currOccupyUnit = null;
        }
    }
    internal override void OnDisable()
    {
        base.OnDisable();
        isOccupied = false;
        currOccupyUnit = null;
        OnPositionRemove?.Invoke(this);
    }
    private void Update()
    {
        if (lastOccupied != isOccupied)
        {
            OnPositionOccupied?.Invoke();
            lastOccupied = isOccupied;
        }
    }
}