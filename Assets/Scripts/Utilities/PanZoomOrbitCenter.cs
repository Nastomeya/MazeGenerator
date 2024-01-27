//Reference: https://www.youtube.com/watch?v=qEpmOqLHWcA
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoomOrbitCenter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 250.0f;//500.0f;
    [SerializeField] private float zoomScale = 5.0f;
    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 16f;//100.0f;//

    private Camera currCamera;
    private Vector3 panClickedPosStart, orbitClickedPosStart;
    private float verticalInput, horizontalInput;

    private bool IsMiddleMouseBtnDown => Input.GetMouseButtonDown(2);
    private bool IsMiddleMouseButton => Input.GetMouseButton(2);
    private bool IsRightMouseButton => Input.GetMouseButton(1);
    private bool IsRightMouseBtnDown => Input.GetMouseButtonDown(1);

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

        if (IsRightMouseBtnDown)
        {
            var ray = currCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            // Check if the clicked object has a collider
            if (hit.collider == null)
                return;

            orbitClickedPosStart = hit.point;
        }
        verticalInput = MouseYAxis * rotationSpeed * Time.deltaTime;
        horizontalInput = MouseXAxis * rotationSpeed * Time.deltaTime;

        // Set the clicked object as the new target
        transform.RotateAround(orbitClickedPosStart, Vector3.up, horizontalInput);
        transform.RotateAround(orbitClickedPosStart, transform.right, -verticalInput);
    }
    private void Pan()
    {
        if (!IsMiddleMouseButton)
            return;

        if (IsMiddleMouseBtnDown) //Check for Pan
            panClickedPosStart = currCamera.ScreenToWorldPoint(Input.mousePosition);

        if (MouseYAxis != 0 || MouseXAxis != 0)
        {
            Vector3 mouseWorldPosDiff = panClickedPosStart - currCamera.ScreenToWorldPoint(Input.mousePosition);
            currCamera.transform.position += mouseWorldPosDiff;
        }
    }
    private void Zoom()
    {
        if (MouseScroll == 0)
            return;

        panClickedPosStart = currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize - MouseScroll * zoomScale, zoomMin, zoomMax);
        Vector3 mouseWorldPosDiff = panClickedPosStart - currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.transform.position += mouseWorldPosDiff;
    }
}