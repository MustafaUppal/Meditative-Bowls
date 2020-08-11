using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bowl : Item
{
    public Color lightColor;
    public Material material;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        transform.GetChild(0).GetComponent<Light>().color = lightColor;
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
