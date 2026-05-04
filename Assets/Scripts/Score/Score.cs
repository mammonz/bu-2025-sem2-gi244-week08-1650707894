using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score instance;

    public int coinValue = 10;
    public float scrollSpeed = 5f;

    public TextMeshProUGUI scoreText;

    private float currentScore = 0f;
    private bool isDead = false;
    public Result result;
    private void Awake()
    {
        instance = this;
        result.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isDead)
        {
            currentScore += scrollSpeed * Time.deltaTime;
            scoreText.text = $"Score: {currentScore:0}";
        }
    }

    public void CoinScore()
    {
        currentScore += coinValue;
        scoreText.text = $"Score: {currentScore:0}";
    }

    public void OnPlayerDeath()
    {
        if (isDead) return;

        isDead = true;

        scoreText.gameObject.SetActive(false);

        if (result != null)
        {
            result.gameObject.SetActive(true);
            result.DisplayResults(currentScore);
        }
    }
}
