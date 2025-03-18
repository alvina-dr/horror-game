using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputManager : MonoBehaviour
{
    public CharacterController CharacterController;
    public CinemachineCamera CinemachineCamera;
    [SerializeField] private AudioSource _footstepAudioSource;
    private Vector3 _moveDirection;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private PlayerGPCamera _playerGPCamera;

    [SerializeField] private float _gravity;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);

        if (_moveDirection != Vector3.zero && !_footstepAudioSource.isPlaying && !_playerGPCamera.IsUsingCamera)
        {
            _footstepAudioSource.Play();
        }
        else if (_moveDirection == Vector3.zero) 
        {
            _footstepAudioSource.Stop();
        }
    }

    public void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        //transform.forward = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if (_playerGPCamera.IsUsingCamera)
        {
            _playerGPCamera.TakePicture();
        }
        else
        {
            _playerGPCamera.OpenCamera();
        }
    }

    private void FixedUpdate()
    {
        Vector3 rotatedDirection = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(_moveDirection.x, 0, _moveDirection.y);

        //Vector3 vector = new Vector3(_moveDirection.x * transform.forward.x, 0, _moveDirection.y * transform.forward.z);
        if (!_playerGPCamera.IsUsingCamera)
        {
            CharacterController.Move(rotatedDirection * _moveSpeed);
        }

        CharacterController.Move(Vector3.down * _gravity);
    }
}
