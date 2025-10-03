using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int startingAmmo = 10;

    private int ammo;
    private int killCount;
    private bool isGameOver;
    private bool isGameStarted;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ---------------- GAME FLOW ----------------
    public void StartGame()
    {
        ammo = startingAmmo;
        killCount = 0;
        isGameOver = false;
        isGameStarted = true;

        GameEvents.OnAmmoChanged?.Invoke(ammo);
        GameEvents.OnKillCountChanged?.Invoke(killCount);
        GameEvents.OnGameStart?.Invoke();

        Debug.Log("Game Started!");
    }

    private void GameOver()
    {
        isGameOver = true;
        isGameStarted = false;


        GameEvents.OnGameOver?.Invoke();

        Debug.Log("Game Over!");
    }

    // ---------------- STATE ----------------
    public bool IsGameOver() => isGameOver;
    public bool IsGameStarted() => isGameStarted;
    public bool IsGameActive() => isGameStarted && !isGameOver;

    // ---------------- AMMO ----------------
    public bool HasAmmo() => ammo > 0;

    public void UseAmmo()
    {
        AudioManager.Instance.PlaySFX("shoot");
        if (ammo <= 0 || isGameOver) return;

        ammo--;
        GameEvents.OnAmmoChanged?.Invoke(ammo);

        if (ammo <= 0)
            GameOver();
    }

    public void AddAmmo(int amount)
    {
        if (isGameOver) return;

        ammo += amount;
        GameEvents.OnAmmoChanged?.Invoke(ammo);
        AudioManager.Instance.PlaySFX("special");
    }

    public int GetAmmo() => ammo;

    // ---------------- KILLS ----------------
    public void AddKill()
    {
        if (isGameOver) return;

        killCount++;
        GameEvents.OnKillCountChanged?.Invoke(killCount);

        AudioManager.Instance.PlaySFX("kill");
    }

    public int GetKillCount() => killCount;
}
