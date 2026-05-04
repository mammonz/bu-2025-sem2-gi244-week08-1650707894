using UnityEngine;
using System.IO;
public class System_Highest_score : MonoBehaviour
{
    public static System_Highest_score instance;

    private string savePath;
    private Data_HighScore saveData;
    public float GetBestScore() => saveData.highestScore;

    private void Awake()
    {
        instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "highest_score.json");
        LoadHighscore();
    }
    public void SaveProcess(float finalScore, out bool isNewRecord)
    {
        isNewRecord = false;

        if (finalScore > saveData.highestScore)
        {
            saveData.highestScore = finalScore;
            isNewRecord = true;
            SaveHighscore();
        }
    }
    private void SaveHighscore()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }
    private void LoadHighscore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<Data_HighScore>(json);
        }
        else
        {
            saveData = new Data_HighScore { highestScore = 0 };
        }
    }
}
