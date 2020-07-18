using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRefs : MonoBehaviour
{
    public static AllRefs I;

    private void Awake()
    {
        I = this;
    }
    
    [Header("Menus")]
    public DockEventListener dock;
}
