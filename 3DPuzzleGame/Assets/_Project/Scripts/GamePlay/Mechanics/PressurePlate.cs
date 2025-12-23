using UnityEngine;

// ============================================================================
// PRESSURE PLATE
// ============================================================================
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Transform _plateTransform;
    [SerializeField] private float _pressDepth = 0.1f;
    [SerializeField] private float _pressSpeed = 5f;
    [SerializeField] private UnityEngine.Events.UnityEvent _onActivate;
    [SerializeField] private UnityEngine.Events.UnityEvent _onDeactivate;

    private bool _isPressed = false;
    private Vector3 _upPosition;
    private Vector3 _downPosition;
    private int _objectsOnPlate = 0;

    void Awake()
    {
        _upPosition = _plateTransform.localPosition;
        _downPosition = _upPosition - Vector3.up * _pressDepth;
    }

    void Update()
    {
        Vector3 targetPos = _isPressed ? _downPosition : _upPosition;
        _plateTransform.localPosition = Vector3.Lerp(
            _plateTransform.localPosition, 
            targetPos, 
            Time.deltaTime * _pressSpeed
        );
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " stepped on Pressure Plate!");
        if (other.CompareTag("Player") /*|| other.CompareTag("Movable")*/)
        {
            Debug.Log("Object stepped on Pressure Plate!");
            _objectsOnPlate++;
            if (!_isPressed)
            {
                _isPressed = true;
                _onActivate?.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") /*|| other.CompareTag("Movable")*/)
        {
            _objectsOnPlate--;
            if (_objectsOnPlate <= 0)
            {
                _objectsOnPlate = 0;
                _isPressed = false;
                _onDeactivate?.Invoke();
            }
        }
    }
}
