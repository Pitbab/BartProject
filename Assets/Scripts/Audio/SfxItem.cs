using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxItem : PoolItem
{
    [SerializeField] private AudioSource Source;

    public void PlayClip(AudioClip clip, float volume = 0.5f)
    {
        Source.clip = clip;
        Source.volume = volume;
        Source.Play();

        StartCoroutine(EndClip());
    }

    private IEnumerator EndClip()
    {
        yield return new WaitForSeconds(Source.clip.length);
        Source.Stop();
        Source.clip = null;
        Remove();
    }
}
