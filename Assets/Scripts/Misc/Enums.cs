
public enum AnimationType
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    Wall,
    Yeet,
    Stun,
    Sniff,
    Win
}

// *** Order in CharType and SpawnPointType must be equal!!! ***
public enum CharType
{
    Cat,
    Mouse
}

public enum SpawnPointType
{
    Cat,
    Mouse,
    Cheese
}

public enum GameState
{
    MainMenu,
    PlayerSelect,
    Options,
    Credits,
    LevelRunning,
    LevelEnd,
    Pause
}