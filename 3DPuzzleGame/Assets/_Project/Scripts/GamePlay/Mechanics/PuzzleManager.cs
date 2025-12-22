using UnityEngine;

// ============================================================================
// PUZZLE MANAGER
// ============================================================================
public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleCondition
    {
        public string conditionName;
        public bool isMet;
    }

    [SerializeField] private PuzzleCondition[] _conditions;
    [SerializeField] private UnityEngine.Events.UnityEvent _onPuzzleComplete;

    private bool _puzzleCompleted = false;

    public void SetCondition(string conditionName, bool value)
    {
        foreach (var condition in _conditions)
        {
            if (condition.conditionName == conditionName)
            {
                condition.isMet = value;
                CheckPuzzleCompletion();
                return;
            }
        }
    }

    private void CheckPuzzleCompletion()
    {
        if (_puzzleCompleted) return;

        foreach (var condition in _conditions)
        {
            if (!condition.isMet)
                return;
        }

        CompletePuzzle();
    }

    private void CompletePuzzle()
    {
        _puzzleCompleted = true;
        _onPuzzleComplete?.Invoke();
        UIManager.Instance?.ShowMessage("Puzzle Solved!", 3f);
        PuzzleGameEvents.OnPuzzleCompleted?.Invoke(gameObject.name);
    }
}