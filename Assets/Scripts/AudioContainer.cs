using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "AudioContainer")]
public class AudioContainer : ScriptableObject
{
    [SerializeField] private AudioClip HurtSfx;
    [SerializeField] private AudioClip DeadSfx;
    [SerializeField] private AudioClip WalkingSfx;
}
