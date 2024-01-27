using JasonPoolSystem;
using UnityEngine;
[RequireComponent(typeof(TextMeshOnObject))]
public abstract class ObjectBase : PooledMonoBehaviour
{
    private TextMeshOnObject textMeshOnObj;
    protected virtual void Awake()
    {
        textMeshOnObj = GetComponent<TextMeshOnObject>();
    }
    internal abstract void SetSize(float _size);
    internal abstract float GetSize();
    internal void SubscribedToOnDisplayOnOff()
    {
        if (textMeshOnObj is TrackText trackText)
            trackText.Init();
    }
    internal void UnsubscribedToOnDisplayOnOff()
    {
        if (textMeshOnObj is TrackText trackText)
            trackText?.End();
    }
    internal void SetNumberOnObject(int number) => textMeshOnObj.textMesh.text = number.ToString();

}