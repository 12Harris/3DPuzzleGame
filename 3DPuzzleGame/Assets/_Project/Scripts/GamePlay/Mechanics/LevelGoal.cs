using UnityEngine;

// ============================================================================
// LEVEL GOAL
// ============================================================================
public class LevelGoal : MonoBehaviour, IInteractable
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private bool _requiresAllPuzzles = false;
    
    private int _puzzlesSolved = 0;
    private int _totalPuzzles = 0;

    void Awake()
    {
        _totalPuzzles = FindObjectsOfType<PuzzleManager>().Length;
        PuzzleGameEvents.OnPuzzleCompleted?.Subscribe(OnPuzzleSolved);
    }

    public string GetInteractionPrompt()
    {
        if (_requiresAllPuzzles && _puzzlesSolved < _totalPuzzles)
            return $"[E] Locked - Solve puzzles ({_puzzlesSolved}/{_totalPuzzles})";
        return "[E] Complete Level";
    }

    public void Interact(GameObject interactor)
    {
        if (_requiresAllPuzzles && _puzzlesSolved < _totalPuzzles)
        {
            UIManager.Instance?.ShowMessage($"Solve all puzzles first! ({_puzzlesSolved}/{_totalPuzzles})", 2f);
            return;
        }

        CompleteLevel();
    }

    public bool CanInteract() => true;

    private void OnPuzzleSolved(string puzzleName)
    {
        _puzzlesSolved++;
        UIManager.Instance?.ShowMessage($"Puzzles Solved: {_puzzlesSolved}/{_totalPuzzles}", 2f);
    }

    private void CompleteLevel()
    {
        UIManager.Instance?.ShowMessage("Level Complete!", 2f);
        
        if (!string.IsNullOrEmpty(_nextSceneName))
        {
            SceneLoader.Instance?.LoadScene(_nextSceneName);
        }
    }

    void OnDestroy()
    {
        PuzzleGameEvents.OnPuzzleCompleted?.Unsubscribe(OnPuzzleSolved);
    }
}