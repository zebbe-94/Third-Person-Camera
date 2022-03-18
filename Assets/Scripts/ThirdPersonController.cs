using UnityEngine;

// This controller is very minimalistic, primarily to show of the camera
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ThirdPersonCamera _thirdPersonCamera;
    private Transform _cameraTransform;

    private void Awake()
    {
        InitializeReferences();
        _cameraTransform = _thirdPersonCamera.Camera.transform;
    }

    private void Reset()
    {
        InitializeReferences();
    }

    void Update()
    {
        Vector3 cameraDirection = _cameraTransform.forward;
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion rotation = Quaternion.LookRotation(new Vector3(cameraDirection.x, 0, cameraDirection.z));
        Vector3 moveVector = transform.position + rotation * input * _speed * Time.deltaTime; 
        
        _rigidbody.MovePosition(moveVector);
        if (moveVector != transform.position)
        {
            transform.rotation = rotation;
        }
    }

    private void InitializeReferences()
    {
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>() == null
                ? gameObject.AddComponent<Rigidbody>()
                : gameObject.GetComponent<Rigidbody>();
        }

        if (_thirdPersonCamera == null)
        {
            _thirdPersonCamera = gameObject.GetComponent<ThirdPersonCamera>() == null
                ? gameObject.AddComponent<ThirdPersonCamera>()
                : gameObject.GetComponent<ThirdPersonCamera>();
        }
    }
}