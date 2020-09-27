using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanningValueInitializer : MonoBehaviour
{

    void Start()
    {
        for (int i = 0; i <= InventoryManager.Instance.bowlsManager.activeBowlsIndexes.Length - 1; i++)
        {
            if (InventoryManager.Instance.bowlsManager.activeBowlsIndexes[i] != -1)
            {
                this.transform.GetChild(i).GetComponent<Triggers>().DefaultValueOfPanning =
                   InventoryManager.Instance.allBowls[InventoryManager.Instance.bowlsManager.
                   activeBowlsIndexes[i]].GetComponent<AudioSource>().panStereo;
            }
        }
    }
}
