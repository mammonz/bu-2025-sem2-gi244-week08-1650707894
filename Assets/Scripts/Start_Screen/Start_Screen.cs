using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Screen : MonoBehaviour
{
    public string loadScene;
    public void On_Start()
    {
        SceneManager.LoadScene(loadScene);
    }
}
