using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioCollection : MonoBehaviour
{
    public List<AudioClip> hurtSFX = new List<AudioClip>();
    public List<AudioClip> StepSFX = new List<AudioClip>();
    
    public void PlayStep(Vector3 position, float volume)
    {
        int index = Random.Range(0, StepSFX.Count);
        AudioSource.PlayClipAtPoint(StepSFX[index], position, volume);
    }

    public void PlayHurt(Vector3 position, float volume)
    {
        int index = Random.Range(0, hurtSFX.Count);
        AudioSource.PlayClipAtPoint(hurtSFX[index], position, volume);
    }
}
