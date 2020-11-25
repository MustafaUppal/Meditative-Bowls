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
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<AudioLightSync>().emit = true;

        if(!gameObject.activeInHierarchy) return;

        audioSource.Stop();
        audioSource.Play();

        InventoryManager.Instance.bowlsManager.AddPlayingAudio(base.Index, AudioSource);
    }
}
