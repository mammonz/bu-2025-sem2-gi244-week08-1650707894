using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI zoneText;
    public GameObject speedBoostIcon;
    public GameObject immortalityIcon;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalDistanceText;
    public TextMeshProUGUI finalCoinText;
    public TextMeshProUGUI bestDistanceText;
    public TextMeshProUGUI bestCoinText;
    public Button restartButton;

    private bool gameOverShown;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestart);

        gameOverShown = false;
    }

    private void Update()
    {
        UpdateHUD();
        UpdateItemIcons();
        CheckGameOver();
    }

    private void UpdateHUD()
    {
        if (ScoreManager.Instance == null) return;

        if (distanceText != null)
            distanceText.text = Mathf.FloorToInt(ScoreManager.Instance.Distance) + " m";

        if (coinText != null)
            coinText.text = "Coin : " + ScoreManager.Instance.Coins.ToString();

        if (zoneText != null && GameManager.Instance != null)
        {
            switch (GameManager.Instance.CurrentZone)
            {
                case ZoneType.Normal:
                    zoneText.text = "";
                    break;
                case ZoneType.UpsideDown:
                    zoneText.text = "UPSIDE DOWN!";
                    break;
                case ZoneType.TwoFloor:
                    zoneText.text = "TWO FLOORS!";
                    break;
            }
        }
    }

    private void UpdateItemIcons()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        PlayerController pc = player.GetComponent<PlayerController>();
        if (speedBoostIcon != null)
            speedBoostIcon.SetActive(pc.HasSpeedBoost);
        if (immortalityIcon != null)
            immortalityIcon.SetActive(pc.IsImmortal);
    }

    private void CheckGameOver()
    {
        if (gameOverShown) return;

        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        if (player.GetComponent<PlayerController>().gameOver)
        {
            gameOverShown = true;
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel == null) return;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CheckAndSaveHighscore();

            if (finalDistanceText != null)
                finalDistanceText.text = Mathf.FloorToInt(ScoreManager.Instance.Distance) + " m";
            if (finalCoinText != null)
                finalCoinText.text = ScoreManager.Instance.Coins.ToString();
            if (bestDistanceText != null)
                bestDistanceText.text = Mathf.FloorToInt(ScoreManager.Instance.BestDistance) + " m";
            if (bestCoinText != null)
                bestCoinText.text = ScoreManager.Instance.BestCoins.ToString();

        }

        gameOverPanel.SetActive(true);
    }

    private void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
