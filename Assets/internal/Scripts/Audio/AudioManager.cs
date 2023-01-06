using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _audioEffect;

    public void ToggleMusic(bool toggle)
    {
        if (toggle)
        {
            _music.Play();
        }
        else
        {
            _music.Stop();
        }
    }


    public void PlayEffectOnce()
    { 
    
    }

    public void PlayEffectLoop()
    {
    
    }

}
