using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] [Tooltip("Will default to main camera if nothing is put in here")]
    private Camera _camera;
    public Camera Camera => _camera;

    [SerializeField] [Tooltip("The pivot point the camera will rotate around and look at")]
    private Transform _pivot;

    [SerializeField] [Tooltip("Modifies the camera rotation speed")]
    private float _rotationSpeed = 1f;
    
    [SerializeField] [Tooltip("Modifies the camera zoom speed")]
    private float _zoomSpeed = 3f;

    [SerializeField] [Tooltip("The distance between the camera and the pivot")]
    private float _defaultDistanceFromPivot = 2f;

    [SerializeField] [Tooltip("The minimum distance between the camera and the pivot")]
    private float _minDistanceFromPivot = 1f;

    [SerializeField] [Tooltip("The maximum distance between the camera and the pivot")]
    private float _maxDistanceFromPivot = 10f;

    [SerializeField] [Range(-89f, 89f)] [Tooltip("Starting angle of camera in degrees")]
    private float _defaultAngle = 30f;

    [SerializeField] [Range(-89f, 89f)] [Tooltip("Minimum angle of camera in degrees")]
    private float _minAngle = -80f;

    [SerializeField] [Range(-89f, 89f)] [Tooltip("Maximum angle of camera in degrees")]
    private float _maxAngle = 80f;

    [SerializeField] [Tooltip("How close the back of the camera can be to colliders")]
    private float _cameraDistanceFromColliders = 0.2f;

    private float _distanceFromTarget;
    private float _verticalAngle;
    private float _horizontalAngle;


    private void Awake()
    {
        InitializeReferences();
        _distanceFromTarget = _defaultDistanceFromPivot;
        _verticalAngle = _defaultAngle;
        _horizontalAngle = Vector3.SignedAngle(Vector3.forward, _pivot.forward, Vector3.up);
        UpdateCameraPosition();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Reset()
    {
        InitializeReferences();
    }

    private void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        _horizontalAngle += Input.GetAxis("Mouse X") * _rotationSpeed;
        _verticalAngle -= Input.GetAxis("Mouse Y") * _rotationSpeed;
        _horizontalAngle %= 360f;
        _verticalAngle = Mathf.Clamp(_verticalAngle, _minAngle, _maxAngle);
        Quaternion rotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);

        _distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        _distanceFromTarget = Mathf.Clamp(_distanceFromTarget, _minDistanceFromPivot, _maxDistanceFromPivot);

        Vector3 cameraDirection = rotation * -Vector3.forward * _distanceFromTarget;
        if (Physics.Raycast(_pivot.position, cameraDirection, out RaycastHit hitInfo, cameraDirection.magnitude + _cameraDistanceFromColliders))
        {
            cameraDirection *= Mathf.Clamp(hitInfo.distance - _cameraDistanceFromColliders, 0, Mathf.Infinity) / cameraDirection.magnitude;
        }
        
        Transform cameraTransform = _camera.transform;
        cameraTransform.position = _pivot.position + cameraDirection;
        cameraTransform.forward = -cameraDirection;
    }

    private void InitializeReferences()
    {
        if (_pivot == null)
        {
            _pivot = transform;
        }

        if (_camera == null)
        {
            _camera = Camera.main == null 
                ? new GameObject("Camera").AddComponent<Camera>() 
                : Camera.main;
        }
    }
}