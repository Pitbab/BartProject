using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "AudioContainer")]
public class AudioContainer : ScriptableObject
{
    public List<AudioClip> hurtSFX;
    public List<AudioClip> StepSFX;

    public AudioClip DeadSFX;
}


