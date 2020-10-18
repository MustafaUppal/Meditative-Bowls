using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bowl : Item
{

    // Variables
    public int position;
    public Color lightColor;
    public Material material;

    // Private Variables
    AudioSource audioSource;
    float panStereo = -1;
    float volume = -1;

    // Propertise
    public AudioSource AudioSource
    {
        get
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            return audioSource;
        }
    }

    public float PanStereo
    {
        get
        {
            if (panStereo == -1)
                panStereo = GetComponent<AudioSource>().panStereo;

            return panStereo;
        }
        set => panStereo = GetComponent<AudioSource>().panStereo = value;
    }

    public float Volume
    {
        get
        {
            if (volume == -1)
                volume = GetComponent<AudioSource>().volume;

            return volume;
        }
        set => GetComponent<AudioSource>().volume = value;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        transform.GetChild(0).GetComponent<Light>().color = lightColor;
    }

    private void Update()
    {
        if (audioSource)
            gameObject.transform.GetChild(0).gameObject.SetActive(audioSource.isPlaying);
    }

    public void PlaySound()
    {
        if (!audioSource.isPlaying && !(AllRefs.I._GameManager.state == GameManager.State.Randomization))
        {
            audioSource.Play();
        }
        else if ((AllRefs.I._GameManager.state == GameManager.State.Randomization) && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
