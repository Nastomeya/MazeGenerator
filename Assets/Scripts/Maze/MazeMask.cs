using UnityEngine;

public class MazeMask : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        if (RevealObjectController.Instance != null)
            RevealObjectController.Instance.OnDisplayOnOff += SetOnOff;
    }
    private void Start()
    {
        RevealObjectController.Instance.OnDisplayOnOff += SetOnOff;
    }
    private void OnDisable()
    {
        RevealObjectController.Instance.OnDisplayOnOff -= SetOnOff;
    }
    internal void SetOnOff(bool _isOnOff)
    {
        meshRenderer.enabled = _isOnOff;
    }
}
