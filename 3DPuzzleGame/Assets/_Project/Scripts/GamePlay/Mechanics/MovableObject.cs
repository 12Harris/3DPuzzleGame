// ============================================================================
// MOVABLE OBJECT (for puzzles)
// ============================================================================
using UnityEngine;
public class MovableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private float _pushForce = 5f;
    [SerializeField] private float _maxDistance = 10f;
    
    private Rigidbody _rb;
    private bool _isBeingPushed = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (!_rb)
            _rb = gameObject.AddComponent<Rigidbody>();
        
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public string GetInteractionPrompt()
    {
        return "[E] Push Object";
    }

    public void Interact(GameObject interactor)
    {
        Vector3 direction = (transform.position - interactor.transform.position).normalized;
        direction.y = 0;
        _rb.AddForce(direction * _pushForce, ForceMode.Impulse);
    }

    public bool CanInteract() => !_isBeingPushed;
}
