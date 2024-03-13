using UnityEngine;

public abstract class BaseInputHandler : MonoBehaviour
{
    protected Camera currCamera;

    public void Init(Camera _camera) => currCamera = _camera;

    protected void MoveCamera(Vector3 _startingPoint, Vector3 _currentInputScreenPoint)
    {
        var worldPosDifferent = _startingPoint - currCamera.ScreenToWorldPoint(_currentInputScreenPoint);
        currCamera.transform.position += worldPosDifferent;
    }
}