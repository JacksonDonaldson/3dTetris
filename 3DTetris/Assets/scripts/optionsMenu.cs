using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class optionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void setVolume(float volume)
    {
        if (volume == -35)
        {
            audioMixer.SetFloat("musicVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("musicVolume", volume);
        }
    }
}
