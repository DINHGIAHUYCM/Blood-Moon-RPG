using UnityEngine;
using UnityEngine.UI;

public class AdCounter : MonoBehaviour
{
    public int adCountNeeded = 20;
    public GameObject panel;
    public Slider progressSlider;
    public string progressKey = "adProgress"; // Biến PlayerPrefs public mới

    private int adCount = 0;

    private void Start()
    {
        panel.SetActive(false);
        LoadProgress();
        UpdateProgressSlider();
    }

    public void CountAd()
    {
        adCount++;

        UpdateProgressSlider();

        if (adCount >= adCountNeeded)
        {
            panel.SetActive(true);
        }

        SaveProgress();
    }

    private void UpdateProgressSlider()
    {
        float progress = (float)adCount / adCountNeeded;
        progressSlider.value = progress;
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(progressKey, adCount);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        if (PlayerPrefs.HasKey(progressKey))
        {
            adCount = PlayerPrefs.GetInt(progressKey);
        }
    }

    public void ResetProgress()
    {
        adCount = 0;
        PlayerPrefs.DeleteKey(progressKey);
        PlayerPrefs.Save();
        UpdateProgressSlider();
    }
}