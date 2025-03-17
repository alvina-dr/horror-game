using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputManager : MonoBehaviour
{
    public CharacterController CharacterController;
    public CinemachineCamera CinemachineCamera;
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
    }

    public void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        //transform.forward = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 rotatedDirection = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(_moveDirection.x, 0, _moveDirection.y);

        //Vector3 vector = new Vector3(_moveDirection.x * transform.forward.x, 0, _moveDirection.y * transform.forward.z);
        CharacterController.Move(rotatedDirection * _moveSpeed);
        CharacterController.Move(Vector3.down * _gravity);
    }
}
