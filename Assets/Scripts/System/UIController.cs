using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI killText;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject hudPanel;
    public GameObject gameOverPanel;

    void OnEnable()
    {
        GameEvents.OnAmmoChanged += UpdateAmmo;
        GameEvents.OnKillCountChanged += UpdateKill;
        GameEvents.OnGameStart += () => ShowPanel(hudPanel);
        GameEvents.OnGameOver += () => ShowPanel(gameOverPanel);
    }

    void OnDisable()
    {
        GameEvents.OnAmmoChanged -= UpdateAmmo;
        GameEvents.OnKillCountChanged -= UpdateKill;
        GameEvents.OnGameStart -= () => ShowPanel(hudPanel);
        GameEvents.OnGameOver -= () => ShowPanel(gameOverPanel);
    }

    void Start()
    {
        ShowPanel(startPanel); // default state
    }

    // -------- Panel Switching --------
    void ShowPanel(GameObject targetPanel)
    {
        // Hide all panels first
        if (startPanel) startPanel.SetActive(false);
        if (hudPanel) hudPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        // Show the one we want
        if (targetPanel) targetPanel.SetActive(true);
    }

    // -------- UI Updates --------
    void UpdateAmmo(int ammo)
    {
        if (ammoText) ammoText.text = ammo.ToString();
    }

    void UpdateKill(int kills)
    {
        if (killText) killText.text = kills.ToString();
    }

    // -------- Button Hooks --------
    public void OnStartButton() => GameManager.Instance.StartGame();
    public void OnRestartButton() => GameManager.Instance.StartGame();
}
