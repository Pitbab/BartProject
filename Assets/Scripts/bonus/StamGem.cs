using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StamGem : MonoBehaviour
{
    [SerializeField] private float StamGain = 20.0f;
    private float MaxEmission = 3.5f;
    private float TimeToShift = 0.5f;
    private Color CurrentColor = Color.green;
    private Color NextColor = Color.green * 3.5f;
    private Color Temp;

    private Renderer GemRenderer;
    [SerializeField] private AudioClip GemSFX;

    private const string Player = "Player";

    private void Start()
    {
        GemRenderer = GetComponent<Renderer>();

        StartCoroutine(LightShift());
    }

    private IEnumerator LightShift()
    {
        float timer = 0;

        while (timer < TimeToShift)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / TimeToShift;

            GemRenderer.material.color = Color.Lerp(CurrentColor, NextColor, lerpValue);

            yield return null;
        }

        Temp = CurrentColor;
        CurrentColor = NextColor;
        NextColor = Temp;
        
        StartCoroutine(LightShift());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Player))
        {
            PlayerManager.Instance.GetStam(StamGain);
            AudioManager.Instance.PlaySfx(GemSFX, other.transform.position, 1f);
            Destroy(gameObject);
        }
    }
}
