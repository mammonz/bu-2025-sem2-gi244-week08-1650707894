using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI newrecordText;

    private void Awake()
    {
        newrecordText.gameObject.SetActive(false);
    }
    public void DisplayResults(float finalScore)
    {
        bool isNewRecord;

        System_Highest_score.instance.SaveProcess(finalScore, out isNewRecord);

        totalScoreText.text = $"Total Score: {finalScore:0}";
        highScoreText.text = $"Best Score: {System_Highest_score.instance.GetBestScore():0}";

        newrecordText.gameObject.SetActive(isNewRecord);

        if (isNewRecord && newrecordText.TryGetComponent<RainbowText>(out var rainbow))
        {
            rainbow.enabled = true;
        }
    }
}
