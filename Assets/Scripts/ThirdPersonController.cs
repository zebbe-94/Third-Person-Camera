using UnityEngine;

// This controller is only the bare minimum to show of the camera
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _body;
    [SerializeField] private ThirdPersonCamera _thirdPersonCamera;
    private Transform _cameraTransform;

    private void Awake()
    {
        InitializeReferences();
        _cameraTransform = _thirdPersonCamera.GetCamera().transform;
    }

    private void Reset()
    {
        InitializeReferences();
    }

    void Update()
    {
        Vector3 cameraDirection = _cameraTransform.forward;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(cameraDirection.x, 0, cameraDirection.z));
        Vector3 moveVector = transform.position + rotation *
                             new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 
                             _speed * Time.deltaTime;
        _rigidbody.MovePosition(moveVector);
        _body.rotation = rotation;
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