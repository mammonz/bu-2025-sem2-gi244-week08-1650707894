using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject canVas;
    public Toggle soundToggle;
    private AudioSource audioSource;

    public bool isSoundEnabled = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        canVas.SetActive(false);

        if (soundToggle != null)
        {
            soundToggle.isOn = isSoundEnabled;
            soundToggle.onValueChanged.AddListener(OnToggleSound);
        }
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

        audioSource.mute = !isOn;
    }
}
