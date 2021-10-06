using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> Audios;
    [SerializeField] private AudioSource AudioPlayer;

    private void Start()
    {
        AudioPlayer.PlayOneShot(SelectRandomAudio());
    }

    private void Update()
    {
        if (!AudioPlayer.isPlaying)
        {
            SelectRandomAudio();
        }
    }

    private AudioClip SelectRandomAudio()
    {
        int index = Random.Range(0, Audios.Count -1);
        return Audios[index];
    }
    
}
