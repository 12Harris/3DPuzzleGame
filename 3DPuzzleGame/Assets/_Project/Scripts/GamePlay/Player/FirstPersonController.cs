// ============================================================================
// FIRST PERSON CONTROLLER
// ============================================================================
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2.5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Look")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _lookXLimit = 80f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _rotationX;
    private bool _canMove = true;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
        if (!_cameraTransform)
            _cameraTransform = Camera.main.transform;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!_canMove) return;

        CheckGround();
        HandleMovement();
        HandleLook();
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;
    }

    private void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.MoveInput;
        
        // Calculate speed
        float speed = _walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed = _runSpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            speed = _crouchSpeed;

        // Movement
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        _controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

        // Gravity
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 lookInput = InputHandler.Instance.LookInput;
        
        float mouseX = lookInput.x * _mouseSensitivity;
        float mouseY = lookInput.y * _mouseSensitivity;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);

        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void SetCanMove(bool canMove) => _canMove = canMove;
}
