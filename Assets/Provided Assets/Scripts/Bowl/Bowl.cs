using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bowl : Item
{
    public int position;
    public Color lightColor;
    public Material material;
    AudioSource audioSource;
    // public Item.State currentState;
    // int[] startingIndeces = {0, 8, 15, 22};

    
    // public int Position
    // {
    //     get
    //     {
    //         Debug.Log("B | index: " + index + ", set: " + set + ", si: " + startingIndeces[set - 1]);
    //         return index - startingIndeces[set - 1] + 1;
    //     }
    // }

    public AudioSource AudioSource
    {
        get 
        {
            if(audioSource == null)
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
    
        if (!audioSource.isPlaying && !(AllRefs.I._GameManager.state == GameManager.State.Randomization))
        {

            audioSource.Play();
        }
        else if((AllRefs.I._GameManager.state == GameManager.State.Randomization) && audioSource.isPlaying)
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
