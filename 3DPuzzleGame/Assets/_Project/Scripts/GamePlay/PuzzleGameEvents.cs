// ============================================================================
// ADDITIONAL GAME EVENTS
// ============================================================================
public static class PuzzleGameEvents
{
    public static GameEvent<string> OnItemPickedUp = new GameEvent<string>();
    public static GameEvent<string> OnPuzzleCompleted = new GameEvent<string>();
    public static GameEvent<string> OnDoorUnlocked = new GameEvent<string>();
}
