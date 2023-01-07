using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private  AudioSource _music;
    [SerializeField] private  AudioSource _audioEffect;

    [Header("Audio")]
    [SerializeField] private AudioClip _markerAudio;
    [SerializeField] private  AudioClip _walkingAudio;
    [SerializeField] private AudioClip _textAudio;



     private static AudioSource s_music;
     private static AudioSource s_audioEffect;

   
     private static AudioClip s_markerAudio;
     private static AudioClip s_walkingAudio;
     private static AudioClip s_textAudio;


    public void OnAfterDeserialize()
    {
        s_music = _music;
        s_audioEffect = _audioEffect;
        s_markerAudio = _markerAudio;
        s_walkingAudio = _walkingAudio;
        s_textAudio = _textAudio;
    }


        public static void ToggleMusic(bool toggle)
    {
        if (toggle)
        {
            s_music.Play();
        }
        else
        {
            s_music.Stop();
        }
    }


    public static void PlayTextClip()
    {
        s_audioEffect.PlayOneShot(s_textAudio);
    }

    public static void PlayMarkerClip()
    {
        s_audioEffect.PlayOneShot(s_markerAudio);
    }

    public static void PlayWalking()
    {
        s_audioEffect.clip = s_walkingAudio;
        s_audioEffect.loop = true;
       s_audioEffect.Play();
    }

    public static void StopWalking()
    {
        s_audioEffect.loop = false;
        s_audioEffect.Stop();
    }



}
