using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanningValueInitializer : MonoBehaviour
{
    
    void Start()
    {
        for(int i= 0; i<=InventoryManager.Instance.bowlsManager.activeBowlsIndexes.Length-1;i++) 
        {
                this.transform.GetChild(i).GetComponent<Triggers>().DefaultValueOfPanning=
                InventoryManager.Instance.allBowls[InventoryManager.Instance.bowlsManager.
                activeBowlsIndexes[i]].GetComponent<AudioSource>().panStereo;
        }
    }
}
