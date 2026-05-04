using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject canVas;
    public Toggle soundToggle;
    private static Option instance;

    public bool isSoundEnabled = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (canVas != null) canVas.SetActive(false);

        if (soundToggle != null)
        {
            soundToggle.isOn = isSoundEnabled;
            soundToggle.onValueChanged.AddListener(OnToggleSound);
        }

        ApplySoundSettings();
    }

    public void Open_Canvas()
    {
        canVas.SetActive(true);
    }

    public void Close_Canvas()
    {
        canVas.SetActive(false);
    }

    public void OnToggleSound(bool isOn)
    {
        isSoundEnabled = isOn;
        ApplySoundSettings();
    }
    private void ApplySoundSettings()
    {
        AudioListener.volume = isSoundEnabled ? 1f : 0f;
    }
}
