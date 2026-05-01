using UnityEngine;
using UnityEngine.InputSystem;
public class Pause : MonoBehaviour
{
    public GameObject pauseCanvas;
    private void Awake()
    {
        pauseCanvas.SetActive(false);
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}
