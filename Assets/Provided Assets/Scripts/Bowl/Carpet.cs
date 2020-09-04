using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carpet : Item
{
    public Material material;
    // Start is called before the first frame update

    private void Awake()
    {
        Debug.Log(gameObject.name);
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
