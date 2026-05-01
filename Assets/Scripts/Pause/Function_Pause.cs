using UnityEngine;
using UnityEngine.SceneManagement;
public class Function_Pause : MonoBehaviour
{
    public void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
