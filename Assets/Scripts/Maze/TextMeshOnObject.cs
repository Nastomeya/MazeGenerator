using UnityEngine;
using TMPro;

public class TextMeshOnObject : MonoBehaviour
{
    [SerializeField] internal TextMeshPro textMesh;
    internal void Init()
    {
        textMesh.gameObject.SetActive(true);
        RevealObjectController.Instance.OnDisplayOnOff += SetTextOnOff;
    }
    internal void End()
    {
        RevealObjectController.Instance.OnDisplayOnOff -= SetTextOnOff;
    }
    internal void SetTextOnOff(bool _isOnOff)
    {
        textMesh.gameObject.SetActive(_isOnOff);
    }
}