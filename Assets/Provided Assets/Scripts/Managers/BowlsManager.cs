using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlsManager : MonoBehaviour
{
    public static BowlsManager Instance;

    
    // -1 will be assigned if no bowl is on a place
    public int[] activeBowlsIndexes;

    public Vector3[] bowlsPositions;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUpBowls();   
    }

    public List<int> unusedBowls;

    /// <summary>
    /// Reposition all bowls according to active bowls indeces
    /// </summary>
    public void SetUpBowls()
    {
        unusedBowls = new List<int>();

        // Considering all bowls as unused
        for (int i = 0; i < Inventory.Instance.allBowls.Length; i++)
        {
            unusedBowls.Add(i);
        }

        for (int i = 0; i < activeBowlsIndexes.Length; i++)
        {
            // -1 means no bowl is there, so look for next bowl
            if(activeBowlsIndexes[i].Equals(-1))
                continue;

            // Removing used bowls from the list
            unusedBowls.Remove(activeBowlsIndexes[i]);

            // Managing used bowls
            Inventory.Instance.allBowls[activeBowlsIndexes[i]].gameObject.SetActive(true);
            Inventory.Instance.allBowls[activeBowlsIndexes[i]].transform.localPosition = bowlsPositions[i];
            
        }

        // Disabling all used bowls
        for (int i = 0; i < unusedBowls.Count; i++)
        {
            Inventory.Instance.allBowls[unusedBowls[i]].gameObject.SetActive(false);
        }
    }
}
