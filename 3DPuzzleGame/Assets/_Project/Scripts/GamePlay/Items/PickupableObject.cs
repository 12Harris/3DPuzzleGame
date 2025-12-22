using UnityEngine;

// ============================================================================
// PICKUPABLE OBJECT
// ============================================================================
public class PickupableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _itemName = "Object";
    [SerializeField] private bool _isPickedUp = false;

    public string GetInteractionPrompt()
    {
        return $"[E] Pick up {_itemName}";
    }

    public void Interact(GameObject interactor)
    {
        if (!_isPickedUp)
        {
            PickUp();
           PuzzleGameEvents.OnItemPickedUp?.Invoke(_itemName);
        }
    }

    public bool CanInteract() => !_isPickedUp;

    private void PickUp()
    {
        _isPickedUp = true;
        gameObject.SetActive(false);
        // Add to inventory system here
    }
}
