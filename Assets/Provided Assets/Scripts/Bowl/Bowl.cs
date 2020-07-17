using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public AudioSource BowlSound;
    void Start()
    {
        BowlSound = GetComponent<AudioSource>();

    }
    public void PlaySound()
    {
        if (!BowlSound.isPlaying)
        {
            BowlSound.Play();

        }
        gameObject.transform.GetChild(0).gameObject.SetActive(BowlSound.isPlaying);
    }
}
