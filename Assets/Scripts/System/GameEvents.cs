using System;

public static class GameEvents
{
    public static Action<int> OnAmmoChanged;
    public static Action<int> OnKillCountChanged;

    public static Action OnGameStart;
    public static Action OnGameOver;
}
