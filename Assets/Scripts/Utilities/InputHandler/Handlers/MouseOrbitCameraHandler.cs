using UnityEngine;

public class MouseOrbitCameraHandler : BaseInputHandler, IMouseHandler
{
    private bool IsRightMouseButton => Input.GetMouseButton(1);
    private bool IsRightMouseBtnDown => Input.GetMouseButtonDown(1);
    private float MouseXAxis => Input.GetAxis("Mouse X");
    private float MouseYAxis => Input.GetAxis("Mouse Y");

    private Ray ray;
    private Vector3 orbitClickedPosStart;
    private float verticalInput, horizontalInput, rotationSpeed;

    private void Awake()
    {
#if UNITY_EDITOR
        rotationSpeed = 100f;
#elif UNITY_WEBGL
        rotationSpeed = 55f;
#endif
    }

    public void HandleInput()
    {
        if (!IsRightMouseButton)
            return;

        if (IsRightMouseBtnDown)
        {
            ray = currCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            // Check if the clicked object has a collider
            if (hit.collider == null)
                return;

            orbitClickedPosStart = hit.point;
        }

        horizontalInput = MouseXAxis * rotationSpeed * Time.deltaTime;
        verticalInput = MouseYAxis * rotationSpeed * Time.deltaTime;

        // Set the clicked object as the new target
        transform.RotateAround(orbitClickedPosStart, Vector3.up, horizontalInput);
        transform.RotateAround(orbitClickedPosStart, transform.right, -verticalInput);
    }
}