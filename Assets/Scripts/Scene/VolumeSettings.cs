using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start() 
    {
        if(PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("bgmVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetBGMVolume();
            SetSFXVolume();
        }
        
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetBGMVolume()
    {
        float volume = BGMSlider.value;
        myMixer.SetFloat("bgm", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("bgmVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        BGMSlider.value = PlayerPrefs.GetFloat("bgmVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();
    }

}
