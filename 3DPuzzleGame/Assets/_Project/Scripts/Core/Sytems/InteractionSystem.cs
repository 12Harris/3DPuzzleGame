// ============================================================================
// INTERACTION SYSTEM
// ============================================================================
using UnityEngine;
public interface IInteractable
{
    string GetInteractionPrompt();
    void Interact(GameObject interactor);
    bool CanInteract();
}

public class InteractionController : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float _interactionRange = 3f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private Transform _rayOrigin;

    [Header("UI")]
    [SerializeField] private GameObject _interactionPrompt;
    [SerializeField] private TMPro.TextMeshProUGUI _promptText;

    private IInteractable _currentInteractable;
    private GameObject _currentObject;

    void Awake()
    {
        if (!_rayOrigin)
            _rayOrigin = Camera.main.transform;

        InputHandler.Instance.OnInteractPressed += TryInteract;
    }

    void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionRange, _interactableMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null && interactable.CanInteract())
            {
                SetCurrentInteractable(interactable, hit.collider.gameObject);
                return;
            }
        }

        ClearCurrentInteractable();
    }

    private void SetCurrentInteractable(IInteractable interactable, GameObject obj)
    {
        if (_currentInteractable != interactable)
        {
            _currentInteractable = interactable;
            _currentObject = obj;
            ShowPrompt(interactable.GetInteractionPrompt());
        }
    }

    private void ClearCurrentInteractable()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable = null;
            _currentObject = null;
            HidePrompt();
        }
    }

    private void TryInteract()
    {
        if (_currentInteractable != null && _currentInteractable.CanInteract())
        {
            _currentInteractable.Interact(gameObject);
        }
    }

    private void ShowPrompt(string message)
    {
        if (_interactionPrompt && _promptText)
        {
            _interactionPrompt.SetActive(true);
            _promptText.text = message;
        }
    }

    private void HidePrompt()
    {
        if (_interactionPrompt)
            _interactionPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        if (InputHandler.Instance)
            InputHandler.Instance.OnInteractPressed -= TryInteract;
    }
}
