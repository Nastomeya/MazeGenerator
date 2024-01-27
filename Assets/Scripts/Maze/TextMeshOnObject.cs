using UnityEngine;
using TMPro;

public class TextMeshOnObject : MonoBehaviour
{
    [SerializeField] internal TextMeshPro textMesh;
    protected virtual void Start()
    {
        SetTextOnOff(false);
    }
    internal void SetTextOnOff(bool _isOnOff)
    {
        if (textMesh == null)
            return;
        if (textMesh.gameObject == null)
            return;

        textMesh.gameObject.SetActive(_isOnOff);
    }
}