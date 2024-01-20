using UnityEngine;
using TMPro;

public class TextMeshOnObject : MonoBehaviour
{
    [SerializeField] internal TextMeshPro textMesh;
    private void Start()
    {
        SetTextOnOff(true);
    }
    internal void Init()
    {
        RevealObjectController.Instance.OnTextDisplayOnOff -= SetTextOnOff;
        RevealObjectController.Instance.OnTextDisplayOnOff += SetTextOnOff;
    }
    internal void End()
    {
        RevealObjectController.Instance.OnTextDisplayOnOff -= SetTextOnOff;
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