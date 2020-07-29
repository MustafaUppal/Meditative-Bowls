using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : Item
{
    public Material material;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(audioSource.isPlaying);
    }
}
