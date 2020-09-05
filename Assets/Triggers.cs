using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public float DefaultValueOfPanning;
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Bowl")
        other.GetComponent<AudioSource>().panStereo = DefaultValueOfPanning;
    }

}
