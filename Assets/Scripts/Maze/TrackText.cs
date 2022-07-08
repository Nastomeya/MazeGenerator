public class TrackText : TextMeshOnObject
{
    internal void SetNumber(int _number) => textMesh.text = _number.ToString();

    internal void SetMarkerOnOff(bool _isEnable) => gameObject.SetActive(_isEnable);
}