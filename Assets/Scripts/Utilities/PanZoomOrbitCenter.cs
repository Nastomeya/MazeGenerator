using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>Reference: <see href="https://www.youtube.com/watch?v=qEpmOqLHWcA"/> 
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=K_aAnBn5khA"/>
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=5LEVj3PLufE"/>/// </summary>
public class PanZoomOrbitCenter : MonoBehaviour
{
    [SerializeField] private float zoomScale = 5.0f;
    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 16f;//100.0f;//
    [SerializeField] private float touchZoomSpeed = 0.000000005f;
    private float rotationSpeed = 250.0f;//500.0f;

    private Camera currCamera;
    private Transform currCameraTrans;
    private Vector3 panClickedPosStart, orbitClickedPosStart;
    private Vector2 panTouchedPosStart;
    private float verticalInput, horizontalInput;
    private float initTouchesDist = 0f;
    private float currTouchesDist = 0f;
    [SerializeField] internal TextMeshProUGUI touchInfoText;
    private bool IsMiddleMouseBtnDown => Input.GetMouseButtonDown(2);
    private bool IsMiddleMouseButton => Input.GetMouseButton(2);
    private bool IsRightMouseButton => Input.GetMouseButton(1);
    private bool IsRightMouseBtnDown => Input.GetMouseButtonDown(1);

    private float MouseXAxis => Input.GetAxis("Mouse X");
    private float MouseYAxis => Input.GetAxis("Mouse Y");
    private float MouseScroll => Input.GetAxis("Mouse ScrollWheel");

    private void Awake()
    {
        currCamera = GetComponentInChildren<Camera>();
        currCameraTrans = currCamera.transform;

#if UNITY_EDITOR
        rotationSpeed = 100f;
#elif UNITY_WEBGL
        rotationSpeed = 55f;
#endif
    }

    private void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.mousePresent)//not working in andriod browser
        {
            MouseCamOrbit();
            MousePan();
            MouseScrollZoom();
        }
        else if (Input.touchSupported)
            PinchZoomAndTouchPan();
    }

    private void MouseCamOrbit()
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

    private void MousePan()
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

    //private void TouchesZoom()
    //{
    //    if (Input.touchCount != 2)
    //        return;
    //    var touchOne = Input.GetTouch(0);
    //    var touchTwo = Input.GetTouch(1);


    //    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
    //    var touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;


    //    if (touchOne.phase == TouchPhase.Stationary && touchTwo.phase == TouchPhase.Stationary)//if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
    //    {
    //        initTouchesDist = Vector2.Distance(touchOne.position, touchTwo.position);            
    //        //initTouchesDist = (touchOnePrevPos - touchTwoPrevPos).magnitude;


    //    }
    //    else if ((touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved) && currTouchesDist != initTouchesDist)
    //    {
    //        float currTouchesDist = Vector2.Distance(touchOne.position, touchTwo.position);
    //        //currTouchesDist = (touchOne.position - touchTwo.position).magnitude;
    //        // var pinchAmount = (initTouchesDist - currTouchesDist) * touchZoomSpeed;
    //        var pinchAmount = (initTouchesDist - currTouchesDist);


    //        float newOrthographicSize = currCamera.orthographicSize + pinchAmount;


    //        //currCamera.orthographicSize = Mathf.Clamp(newOrthographicSize, zoomMin, zoomMax);
    //        currCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(currCamera.orthographicSize, newOrthographicSize, touchZoomSpeed * Time.deltaTime), zoomMin, zoomMax);

    //        touchInfoText.text = "currTouchesDist: " + currTouchesDist + ",   pinchAmount: " + pinchAmount + ",  currCamera.Size: " + currCamera.orthographicSize;
    //    }
    //    //touchInfoText.text = "Curr Camera.Size: " + currCamera.orthographicSize;

    //}
    //private void TouchesZoom()//a bit better
    //{
    //    if (Input.touchCount != 2)
    //        return;
    //    var touchOne = Input.GetTouch(0);
    //    var touchTwo = Input.GetTouch(1);


    //    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
    //    var touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
    //    float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
    //    float currMagnitude = (touchOne.position - touchTwo.position).magnitude;
    //    //var pinchAmount = (currMagnitude - prevMagnitude) * touchZoomSpeed;
    //    var pinchAmount = (currMagnitude - prevMagnitude);

    //    float newOrthographicSize = currCamera.orthographicSize + pinchAmount;
    //    //currCamera.orthographicSize = Mathf.Clamp(newOrthographicSize, zoomMin, zoomMax);

    //    currCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(currCamera.orthographicSize, newOrthographicSize, touchZoomSpeed * Time.deltaTime), zoomMin, zoomMax);

    //    touchInfoText.text = "Curr Camera.Size: " + currCamera.orthographicSize;
    //}

    //private void TouchesZoom()//not good
    //{
    //    if (touchOne.phase == TouchPhase.Began)
    //        touchInfoText.text = "Began...";
    //    if (touchOne.phase == TouchPhase.Ended)
    //        touchInfoText.text = "Ended...";
    //    if (touchOne.phase == TouchPhase.Moved)
    //        touchInfoText.text = "Moved...";
    //    if (touchOne.phase == TouchPhase.Stationary)
    //        touchInfoText.text = "Stationary...";

    //    if (Input.touchCount != 2)
    //        return;
    //    touchOne = Input.GetTouch(0);
    //    touchTwo = Input.GetTouch(1);


    //    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
    //    var touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
    //    float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
    //    float currMagnitude = (touchOne.position - touchTwo.position).magnitude;

    //    var differ = (currMagnitude - prevMagnitude) * 0.01f;
    //    currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize + differ, zoomMin, zoomMax);


    //    //if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
    //    //{
    //    //    if (currMagnitude > prevMagnitude)
    //    //    {
    //    //        var targetPos = currCameraTrans.position;
    //    //        targetPos.z = Mathf.Lerp(targetPos.z, targetPos.z - 1, Time.deltaTime * touchZoomSpeed);
    //    //        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize + 1, zoomMin, zoomMax);
    //    //        currCameraTrans.position = Vector3.Slerp(currCameraTrans.position, targetPos, Time.deltaTime * touchZoomSpeed);
    //    //    }
    //    //    else if (currMagnitude < prevMagnitude)
    //    //    {
    //    //        var targetPos = currCameraTrans.position;
    //    //        targetPos.z = Mathf.Lerp(targetPos.z, targetPos.z + 1, Time.deltaTime * touchZoomSpeed);
    //    //        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize - 1, zoomMin, zoomMax);
    //    //        currCameraTrans.position = Vector3.Slerp(currCameraTrans.position, targetPos, Time.deltaTime * touchZoomSpeed);
    //    //    }
    //    //}




    //    //touchInfoText.text = "Curr Camera.Size: " + currCamera.orthographicSize;
    //}

    //private void TouchesZoom()//good but keeping reseting
    //{
    //    if (Input.touchCount != 2)
    //        return;
    //    var touchOne = Input.GetTouch(0);
    //    var touchTwo = Input.GetTouch(1);


    //    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
    //    var touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
    //    float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
    //    float currMagnitude = (touchOne.position - touchTwo.position).magnitude;

    //    //var pinchAmount = (currMagnitude - prevMagnitude) * touchZoomSpeed * Time.deltaTime;
    //    var pinchAmount = (currMagnitude - prevMagnitude);
    //    float newOrthographicSize = currCamera.orthographicSize + pinchAmount;
    //    //currCamera.orthographicSize = Mathf.Clamp(newOrthographicSize, zoomMin, zoomMax);
    //    currCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(currCamera.orthographicSize, newOrthographicSize, touchZoomSpeed * Time.deltaTime), zoomMin, zoomMax);

    //    var touchPrevScreenPos = (touchOnePrevPos + touchTwoPrevPos) / 2f;
    //    var touchPrevWorldPos = currCamera.ScreenToWorldPoint(touchPrevScreenPos);
    //    var touchCurrScreenPos = (touchOne.position + touchTwo.position) / 2f;
    //    var touchCurrWorldPos = currCamera.ScreenToWorldPoint(touchCurrScreenPos);


    //    Vector3 touchWorldPosDiff = touchPrevWorldPos - touchCurrWorldPos;
    //    //currCamera.transform.position += touchWorldPosDiff;


    //    var targetPos = currCamera.transform.position + touchWorldPosDiff;
    //    currCamera.transform.position = Vector3.Slerp(currCameraTrans.position, targetPos, touchZoomSpeed * Time.deltaTime);

    //    touchInfoText.text = "Curr Camera.Size: " + currCamera.orthographicSize;
    //}

    private Vector2 initialTouch1Pos;
    private Vector2 initialTouch2Pos;
    private Vector3 initCentreWorldPos;
    private Vector3 currCentreWorldPos;
    private Touch touchOne, touchTwo;
    private void PinchZoomAndTouchPan()
    {
        if (Input.touchCount != 2)
            return;

        touchOne = Input.GetTouch(0);
        touchTwo = Input.GetTouch(1);

        if ((touchOne.phase == TouchPhase.Stationary && touchTwo.phase == TouchPhase.Stationary) || touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
        {
            initialTouch1Pos = touchOne.position;
            initialTouch2Pos = touchTwo.position;
            var initCentreScreenPos = (initialTouch1Pos + initialTouch2Pos) / 2f;
            initCentreWorldPos = currCamera.ScreenToWorldPoint(initCentreScreenPos);
        }
        else if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
        {
            Vector2 currentTouch1Pos = touchOne.position;
            Vector2 currentTouch2Pos = touchTwo.position;

            float initialDistance = Vector2.Distance(initialTouch1Pos, initialTouch2Pos);
            float currentDistance = Vector2.Distance(currentTouch1Pos, currentTouch2Pos);
            float pinchAmount = initialDistance - currentDistance;


            float zoomAmount = pinchAmount * touchZoomSpeed;
            float newOrthographicSize = currCamera.orthographicSize + zoomAmount;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, zoomMin, zoomMax);
            currCamera.orthographicSize = Mathf.Lerp(currCamera.orthographicSize, newOrthographicSize, Time.deltaTime);

            var touchCurrScreenPos = (touchOne.position + touchTwo.position) / 2f;
            var currCentreWorldPos = currCamera.ScreenToWorldPoint(touchCurrScreenPos);

            Vector3 touchWorldPosDiff = initCentreWorldPos - currCentreWorldPos;
            currCamera.transform.position += touchWorldPosDiff;
        }
    }

    private void MouseScrollZoom()
    {
        if (MouseScroll == 0)
            return;

        panClickedPosStart = currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize - MouseScroll * zoomScale, zoomMin, zoomMax);
        Vector3 mouseWorldPosDiff = panClickedPosStart - currCamera.ScreenToWorldPoint(Input.mousePosition);
        currCamera.transform.position += mouseWorldPosDiff;
    }
}