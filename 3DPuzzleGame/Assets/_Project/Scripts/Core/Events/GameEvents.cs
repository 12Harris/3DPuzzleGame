public static class GameEvents
{
    public static GameEvent<int> OnScoreChanged = new GameEvent<int>();
    public static GameEvent<float> OnHealthChanged = new GameEvent<float>();
    public static GameEvent<string> OnGameStateChanged = new GameEvent<string>();
    public static GameEvent<float> OnLoadingProgress = new GameEvent<float>();
}