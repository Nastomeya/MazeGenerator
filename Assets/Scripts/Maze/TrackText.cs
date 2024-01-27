public class TrackText : TextMeshOnObject
{
    protected override void Start()
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
    internal void SetNumber(int _number) => textMesh.text = _number.ToString();

    internal void SetMarkerOnOff(bool _isEnable) => gameObject.SetActive(_isEnable);
}