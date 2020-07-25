using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bowl : MonoBehaviour
{
    public AudioSource BowlSound;
    public Sprite Bowl_Image;
    public string Name;
    [TextArea(2, 4)]
    public string Description;

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
