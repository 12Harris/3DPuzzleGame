// ============================================================================
// FIRST PERSON CONTROLLER
// ============================================================================
using UnityEngine;
using UnityEngine.AI;
using Vault.DataStrucures;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2.5f;
    [SerializeField] private float _currentSpeed = 0f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    private Vector2 _moveDirection = Vector3.zero;
    private Vector2 _moveInput = Vector3.zero;

    [Header("Look")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _lookXLimit = 80f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    //[SerializeField] private float _groundDistance = 0.4f;
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

    void Start()
    {
        //ToDo: Enqueue method with TQueue as parameter
        /*TPriorityQueue<int> queue = new TPriorityQueue<int>();
        queue.Enqueue(0,0);
        queue.Enqueue(1,1);
        queue.Enqueue(2,2);
        queue.Enqueue(3,3);
        queue.Enqueue(4,4);

        queue.Enqueue(0,1,2,3,4);
        queue.Enqueue(5,6,7,8,9);
        queue.Enqueue(10,11,12,13,14);
        queue.Enqueue(15,16,17,18,19);
        queue.Enqueue(20,21,22,23,24);

        //Queue Before
        Debug.Log("Queue Before:");
        Debug.Log(queue);

        //Remove front element
        queue.Dequeue();
        
        //Queue After
        Debug.Log("Queue After:");
        Debug.Log(queue);*/

        BinarySearchTree<int> tree = new BinarySearchTree<int>();
        tree.Insert(0);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(5);
        
        Debug.Log("tree has: " + tree.CountNodes() + " nodes");
        tree.GenerateNodeDisplayTree();

    }

    void Update()
    {
        if (!_canMove) return;

        //CheckGround();
        _isGrounded = IsGrounded();
        HandleMovement();
        HandleLook();

        Debug.Log(_isGrounded? "Grounded" : "Not Grounded");
    }

    //Most robust ground detection for character controller
    bool IsGrounded()
    {
        Vector3 origin = _groundCheck.position + Vector3.up * 0.1f;
        float radius = _controller.radius * 0.9f;

        return _controller.isGrounded ||Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out RaycastHit hit,
            0.2f,
            _groundMask
        );
    }

    private void HandleMovement()
    {
        _moveInput= InputHandler.Instance.MoveInput;

         // Calculate speed
        var targetspeed = 0f;
        if(_moveInput.magnitude > 0f)
        {
        
            targetspeed = _walkSpeed;
            
            if (Input.GetKey(KeyCode.LeftControl))
                targetspeed = _crouchSpeed;

            else if (Input.GetKey(KeyCode.LeftShift))
                targetspeed = _runSpeed;

            
            _moveDirection = _moveInput;

        }
        else 
        {
            targetspeed = 0f;
            if(_currentSpeed <= 0f)
            {
                _moveDirection = Vector3.zero;
            }
        }
        

        if(_moveInput.magnitude > 0.001f || _currentSpeed > 0f)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 
            targetspeed, 
            Time.deltaTime * _acceleration);
        }
    
        // Movement
        //Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        _controller.Move(move *_currentSpeed * Time.deltaTime);

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

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundDistance);
    }*/
}
