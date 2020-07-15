using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public AudioSource BowlSound;
    public Material material;
    void Start()
    {
        material=this.GetComponent<Material>();
        BowlSound=this.GetComponent<AudioSource>();
        
    }
    public void PlaySound()
    {
        if(!BowlSound.isPlaying)
        {
            BowlSound.Play();
            
        }
        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(BowlSound.isPlaying);
    }
}
