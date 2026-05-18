using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshProUGUI coinTxt ;

    private const string HighscoreDistanceKey = "HighscoreDistance";
    private const string HighscoreCoinKey = "HighscoreCoins";

    public float Distance { get; private set; }
    public int Coins { get; private set; }
    public float BestDistance { get; private set; }
    public int BestCoins { get; private set; }
    public bool IsNewRecord { get; private set; }

    [Header("Settings")]
    public float baseSpeed = 10f;

    public event System.Action<float> OnDistanceChanged;
    public event System.Action<int> OnCoinChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadHighscore();
    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null && player.GetComponent<PlayerController>().gameOver)
            return;

        float speedMult = GameManager.Instance != null ? GameManager.Instance.SpeedMultiplier : 1f;
        Distance += baseSpeed * speedMult * Time.deltaTime;
        OnDistanceChanged?.Invoke(Distance);
    }

    public void AddCoin()
    {
        Coins++;
        coinTxt.text = "Coin : " + Coins.ToString(); 
        OnCoinChanged?.Invoke(Coins);
    }

    public void CheckAndSaveHighscore()
    {
        IsNewRecord = false;

        if (Distance > BestDistance)
        {
            BestDistance = Distance;
            PlayerPrefs.SetFloat(HighscoreDistanceKey, BestDistance);
            IsNewRecord = true;
        }

        if (Coins > BestCoins)
        {
            BestCoins = Coins;
            PlayerPrefs.SetInt(HighscoreCoinKey, BestCoins);
            IsNewRecord = true;
        }

        PlayerPrefs.Save();
    }

    private void LoadHighscore()
    {
        BestDistance = PlayerPrefs.GetFloat(HighscoreDistanceKey, 0f);
        BestCoins = PlayerPrefs.GetInt(HighscoreCoinKey, 0);
    }
}
