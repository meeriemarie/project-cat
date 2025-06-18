using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;

public class changeVolume : MonoBehaviour
{
    public Slider thisSlider;
    public RTPC masterVolumeRTPC; // Drag the RTPC object from Wwise Picker into this

    private const string VolumeKey = "GlobalVolume";

    void Start()
    {
        float savedValue = PlayerPrefs.GetFloat(VolumeKey, 1f);
        thisSlider.value = savedValue;

        masterVolumeRTPC.SetGlobalValue(savedValue * 100f);
        thisSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        masterVolumeRTPC.SetGlobalValue(sliderValue * 100f);
        PlayerPrefs.SetFloat(VolumeKey, sliderValue);
    }
}
