using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider     volumeSlider;

    private bool isFullScreen = true;

    private void Start()
    {
        float currentVolume;
        audioMixer.GetFloat("volume", out currentVolume);
        volumeSlider.value = currentVolume;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    List<int> widths = new List<int>() { 1920, 1280, 854, 640 };
    List<int> heights = new List<int>() { 1080, 720, 480, 360 };

    public void SetResolution(int index)
    {
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, isFullScreen);
    }
}