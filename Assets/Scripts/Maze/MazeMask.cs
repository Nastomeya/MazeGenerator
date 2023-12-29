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
        {
            RevealObjectController.Instance.OnMarkDisplayOnOff -= SetOnOff;
            RevealObjectController.Instance.OnMarkDisplayOnOff += SetOnOff;
        }
    }
    private void Start()
    {
        RevealObjectController.Instance.OnMarkDisplayOnOff -= SetOnOff;
        RevealObjectController.Instance.OnMarkDisplayOnOff += SetOnOff;
    }
    private void OnDisable()
    {
        if (RevealObjectController.HasInstance)
            RevealObjectController.Instance.OnMarkDisplayOnOff -= SetOnOff;
    }
    internal void SetOnOff(bool _isOnOff)
    {
        meshRenderer.enabled = _isOnOff;
    }
}
