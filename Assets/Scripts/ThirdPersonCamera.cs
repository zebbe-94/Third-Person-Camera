using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] [Tooltip("Will default to main camera if nothing is put in here")]
    private Camera _camera;

    [SerializeField] [Tooltip("The pivot point the camera will rotate around and look at")]
    private Transform _pivot;

    [SerializeField] [Tooltip("Modifies the camera rotation speed")]
    private float _rotationSpeed = 1f;
    
    [SerializeField] [Tooltip("Modifies the camera zoom speed")]
    private float _zoomSpeed = 3f;

    [SerializeField] [Tooltip("The distance between the camera and the target")]
    private float _defaultDistanceFromPivot = 2f;

    [SerializeField] [Tooltip("The minimum distance between the camera and the target")]
    private float _minDistanceFromPivot = 1f;

    [SerializeField] [Tooltip("The maximum distance between the camera and the target")]
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
    private float _horizontalAngle = 0;


    private void Awake()
    {
        InitializeReferences();
        _distanceFromTarget = _defaultDistanceFromPivot;
        _verticalAngle = _defaultAngle;
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

        Vector3 cameraDirection = rotation * _pivot.forward * -_distanceFromTarget;
        if (Physics.Raycast(_pivot.position, cameraDirection, out RaycastHit hitInfo, cameraDirection.magnitude + _cameraDistanceFromColliders))
        {
            cameraDirection *= Mathf.Clamp(hitInfo.distance - _cameraDistanceFromColliders, 0, Mathf.Infinity) / cameraDirection.magnitude;
        }

        _camera.transform.position = _pivot.position + cameraDirection;
        _camera.transform.LookAt(_pivot);
    }

    private void InitializeReferences()
    {
        if (_pivot == null)
        {
            _pivot = transform;
        }

        if (_camera == null)
        {
            _camera = Camera.main == null ? new GameObject("Camera").AddComponent<Camera>() : Camera.main;
        }
    }

    public Camera GetCamera()
    {
        return _camera;
    }
}