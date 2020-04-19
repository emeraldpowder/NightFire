using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicControls : MonoBehaviour
{
    public AudioMixerGroup Mixer;

    public Text MusicText;
    public Text SoundText;
    
    void Start()
    {
        Mixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetInt("Music", 1) == 1 ? 0 : -80);
        Mixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetInt("Sfx", 1) == 1 ? 0 : -80);

        MusicText.color = PlayerPrefs.GetInt("Music", 1) == 1 ? new Color(0.91f, 0.51f, 0.31f) : Color.gray;
        SoundText.color = PlayerPrefs.GetInt("Sfx", 1) == 1 ? new Color(0.91f, 0.51f, 0.31f) : Color.gray;
    }

    public void ToggleMusic()
    {
        PlayerPrefs.SetInt("Music", 1 - PlayerPrefs.GetInt("Music", 1));
        Mixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetInt("Music", 1) == 1 ? 0 : -80);
        MusicText.color = PlayerPrefs.GetInt("Music", 1) == 1 ? new Color(0.91f, 0.51f, 0.31f) : Color.gray;
    }

    public void ToggleSfx()
    {
        PlayerPrefs.SetInt("Sfx", 1 - PlayerPrefs.GetInt("Sfx", 1));
        Mixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetInt("Sfx", 1) == 1 ? 0 : -80);
        SoundText.color = PlayerPrefs.GetInt("Sfx", 1) == 1 ? new Color(0.91f, 0.51f, 0.31f) : Color.gray;
    }
}
