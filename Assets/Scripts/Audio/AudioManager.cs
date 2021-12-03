using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

     public static AudioManager Instance;

     public List<AudioClip> hurtSFX = new List<AudioClip>();
     public List<AudioClip> StepSFX = new List<AudioClip>();

     public AudioClip DeadSFX;

     public AudioClip BlastSFX;

     public AudioClip InSlowMo;
     public AudioClip OutSlowMo;

     private void Awake()
     {
        if(Instance == null)
        {
             Instance = this;
             DontDestroyOnLoad(gameObject);
        }
        else
        {
             Destroy(gameObject);
        }
     }

     public void PlaySfx(AudioClip clip, Vector3 position, float volume)
     {
          AudioSource.PlayClipAtPoint(clip, position, volume);
     }
     

     
}
