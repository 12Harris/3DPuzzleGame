// ============================================================================
// INPUT HANDLER
// ============================================================================
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class InputHandler : Singleton<InputHandler>
{
    // Input Actions
    public event Action OnJumpPressed;
    public event Action OnAttackPressed;
    public event Action OnInteractPressed;
    public event Action OnPausePressed;
    // Axis values
    private Vector2 _moveInput;
    private Vector2 _lookInput;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        // Legacy Input System
        HandleLegacyInput();
    }

    private void HandleLegacyInput()
    {
        // Movement
        _moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Mouse look
        _lookInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        // Actions
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpPressed?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnAttackPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.E))
            OnInteractPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape))
            OnPausePressed?.Invoke();
    }

    // New Input System methods (requires Input System package)
    /*
    private PlayerInputActions _inputActions;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new PlayerInputActions();
        
        _inputActions.Player.Jump.performed += ctx => OnJumpPressed?.Invoke();
        _inputActions.Player.Attack.performed += ctx => OnAttackPressed?.Invoke();
        _inputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
        _inputActions.UI.Pause.performed += ctx => OnPausePressed?.Invoke();
    }

    void OnEnable() => _inputActions?.Enable();
    void OnDisable() => _inputActions?.Disable();

    public Vector2 GetMoveInput() => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 GetLookInput() => _inputActions.Player.Look.ReadValue<Vector2>();
    */
}