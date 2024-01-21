//Reference: https://www.youtube.com/watch?v=qEpmOqLHWcA
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoomOrbitCenter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 250.0f;//500.0f;
    [SerializeField] private float zoomScale = 5.0f;
    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 100.0f;//16f;

    private Camera currCamera;
    private Vector3 mouseWorldPosStart;
    private float verticalInput, horizontalInput;

    private bool IsMiddleMouseBtnDown => Input.GetMouseButtonDown(2);
    private bool IsMiddleMouseButton => Input.GetMouseButton(2);
    private bool IsRightMouseButton => Input.GetMouseButton(1);
    private float MouseXAxis => Input.GetAxis("Mouse X");
    private float MouseYAxis => Input.GetAxis("Mouse Y");
    private float MouseScroll => Input.GetAxis("Mouse ScrollWheel");

    private void Awake() => currCamera = GetComponentInChildren<Camera>();

    private void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        CamOrbit();
        Pan();
        Zoom();
    }
    private void CamOrbit()
    {
        if (!IsRightMouseButton)
            return;

        if (MouseXAxis == 0 && MouseYAxis == 0)
            return;

        verticalInput = MouseYAxis * rotationSpeed * Time.deltaTime;
        horizontalInput = MouseXAxis * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.right, -verticalInput);
        transform.Rotate(Vector3.up, horizontalInput, Space.World);
    }
    private void Pan()
    {
        if (!IsMiddleMouseButton)
            return;

        if (IsMiddleMouseBtnDown) //Check for Pan
            mouseWorldPosStart = currCamera.ScreenToWorldPoint(Input.mousePosition);

        if (MouseYAxis != 0 || MouseXAxis != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - currCamera.ScreenToWorldPoint(Input.mousePosition);
            currCamera.transform.position += mouseWorldPosDiff;
        }
    }
    private void Zoom()
    {
        if (MouseScroll == 0)
            return;

        mouseWorldPosStart = currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize - MouseScroll * zoomScale, zoomMin, zoomMax);
        Vector3 mouseWorldPosDiff = mouseWorldPosStart - currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.transform.position += mouseWorldPosDiff;
    }
}