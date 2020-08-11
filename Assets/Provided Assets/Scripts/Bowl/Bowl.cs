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
    private void Update()
    {
        if(audioSource)
        gameObject.transform.GetChild(0).gameObject.SetActive(audioSource.isPlaying);
    }
    public void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
