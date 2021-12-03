using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    private const string Player = "Player";
    private const float EndTimer = 1f;

    [SerializeField] private AudioClip FinishedSound;
    private AudioSource Source;

    private bool IsTriggered = false;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Player)
        {
            //to be sure the ending is not triggered multiple time
            if (!IsTriggered)
            {
                IsTriggered = true;
                PlayerManager.Instance.SavingFinalTime();
                StartCoroutine(Finished());
            }
        }
    }

    private IEnumerator Finished()
    {
        GameObject music = GameObject.Find("Music");

        if (music != null)
        {
            music.GetComponent<AudioSource>().Stop();
        }
        
        Source.PlayOneShot(FinishedSound);
        yield return new WaitForSeconds(0.2f);
        Transition.Instance.ChangeScene("Win");
    }



}
