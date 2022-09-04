using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{
    // Set the min and the max size for this 
    // camera so that we can fix the limits
    // of our zoom in and zoom out.
    [SerializeField]
    float CameraSizeMin = 1.0f;
    [SerializeField]
    float CameraSizeMax = 10.0f;

    // A property to allow/disallow 
    // panning of camera.
    // You can set panning to be false
    // if you want to disable panning.
    // One exmaple could be when you are
    // dragung or interacting with a game object 
    // then you can disable camera panning.
    public static bool IsCameraPanning
    {
        get;
        set;
    } = true;

    // Some variables needed for dragging our 
    // camera to creae the pan control
    private Vector3 mDragPos;
    private Vector3 mOriginalPosition;
    private bool mDragging = false;

    // The zoom factor
    private float mZoomFactor = 0.0f;

    // Save a reference to the Camera.main
    private Camera mCamera;

    void Start()
    {
        if (CameraSizeMax < CameraSizeMin)
        {
            float tmp = CameraSizeMax;
            CameraSizeMax = CameraSizeMin;
            CameraSizeMin = tmp;
        }

        if (CameraSizeMax - CameraSizeMin < 0.01f)
        {
            CameraSizeMax += 0.1f;
        }

        SetCamera(Camera.main);
    }

    public void SetCamera(Camera camera)
    {
        mCamera = camera;
        mOriginalPosition = mCamera.transform.position;

        mZoomFactor =
          (CameraSizeMax - mCamera.orthographicSize) /
          (CameraSizeMax - CameraSizeMin);
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            ZoomIn();
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            ZoomOut();
        }

        // Camera panning is disabled when a tile is selected.
        if (!IsCameraPanning)
        {
            mDragging = false;
            return;
        }

        // We also check if the pointer is not on UI item
        // or is disabled.
        if (EventSystem.current.IsPointerOverGameObject() ||
          enabled == false)
        {
            //mDragging = false;
            return;
        }

        // Save the position in worldspace.
        if (Input.GetMouseButtonDown(0))
        {
            mDragPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            mDragging = true;
        }

        if (Input.GetMouseButton(0) && mDragging)
        {
            Vector3 diff = mDragPos - mCamera.ScreenToWorldPoint(Input.mousePosition);
            diff.z = 0.0f;
            float xRightBoundary = GameManager.Instance.gameSize.x / 2;
            float xLeftBoundary = -1 * xRightBoundary;
            float yTopBoundary = GameManager.Instance.gameSize.y / 2;
            float yBottomBoundary = -1 * yTopBoundary;

            Vector3 cameraPositionWithNew = mCamera.transform.position + diff;
            bool respectsXGameSize = cameraPositionWithNew.x > xLeftBoundary && cameraPositionWithNew.x < xRightBoundary;
            bool respectsYGameSize = cameraPositionWithNew.y > yBottomBoundary && cameraPositionWithNew.y < yTopBoundary;
            if (respectsXGameSize && respectsYGameSize)
            {
                mCamera.transform.position += diff;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            mDragging = false;
        }
    }

    public void ResetCameraView()
    {
        mCamera.transform.position = mOriginalPosition;
        mCamera.orthographicSize = CameraSizeMax;
        mZoomFactor = 0.0f;
    }

    public void Zoom(float value)
    {
        mZoomFactor = value;
        // clamp the value between 0 and 1.
        mZoomFactor = Mathf.Clamp01(mZoomFactor);

        // set the camera size
        mCamera.orthographicSize = CameraSizeMax -
            mZoomFactor *
            (CameraSizeMax - CameraSizeMin);
    }

    public void ZoomIn()
    {
        Zoom(mZoomFactor + (CameraSizeMax - CameraSizeMin) * 0.001f);
    }

    public void ZoomOut()
    {
        Zoom(mZoomFactor - (CameraSizeMax - CameraSizeMin) * 0.001f);
    }
}