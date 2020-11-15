using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carpet : Item
{
    public Material material;

    // int[] startingIndeces = {0, 3, 6};

    
    // public int Position
    // {
    //     get
    //     {
    //         Debug.Log("C | index: " + index + ", set: " + set + ", si: " + startingIndeces[set - 1]);
    //         Debug.Log((index - startingIndeces[set - 1] + 1));
    //         return index - startingIndeces[set - 1] + 1;
    //     }
    // }

    // Start is called before the first frame update

    private void Awake()
    {
        // Debug.Log(gameObject.name);
        // Index = int.Parse(gameObject.name.Split('(')[1].Trim(')')) - 1;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
