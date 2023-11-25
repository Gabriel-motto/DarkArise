using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static float volume = .25f;
    public static bool muted = false;
    public Slider volumeSlider;
    public AudioSource backgroundAudio;

    private void Start()
    {
        volumeSlider.value = volume;
    }

    private void Update()
    {
        backgroundAudio.volume = volume;
        backgroundAudio.mute = muted;
    }

    public void setMuted(bool muted)
    {
        AudioManager.muted = muted;
    }

    public void setVolume()
    {
        AudioManager.volume = volumeSlider.value;
    }
}
