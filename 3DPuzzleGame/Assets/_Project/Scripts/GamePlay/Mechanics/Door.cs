using UnityEngine;

// ============================================================================
// DOOR SYSTEM
// ============================================================================
public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isLocked = true;
    [SerializeField] private string _requiredKey = "";
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Vector3 _openRotation = new Vector3(0, 90, 0);
    [SerializeField] private float _openSpeed = 2f;

    private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _targetRotation;

    void Awake()
    {
        _closedRotation = _doorTransform.localRotation;
        _targetRotation = _closedRotation;
    }

    void Update()
    {
        _doorTransform.localRotation = Quaternion.Slerp(
            _doorTransform.localRotation, 
            _targetRotation, 
            Time.deltaTime * _openSpeed
        );
    }

    public string GetInteractionPrompt()
    {
        if (_isLocked)
            return "[E] Locked - Need Key";
        return _isOpen ? "[E] Close Door" : "[E] Open Door";
    }

    public void Interact(GameObject interactor)
    {
        if (_isLocked)
        {
            // Check inventory for key
            UIManager.Instance?.ShowMessage("Door is locked!", 2f);
            return;
        }

        ToggleDoor();
    }

    public bool CanInteract() => true;

    private void ToggleDoor()
    {
        _isOpen = !_isOpen;
        _targetRotation = _isOpen ? 
            Quaternion.Euler(_openRotation) * _closedRotation : 
            _closedRotation;
        
        AudioManager.Instance?.PlaySFX(null); // Add door sound
    }

    public void Unlock()
    {
        _isLocked = false;
        UIManager.Instance?.ShowMessage("Door unlocked!", 2f);
    }
}