using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum State
    {
        Locked,
        Purchased,
        Loaded
    }

    public State currentState;
    public new string name;
    public string set;
    public float price;
    public Sprite image;
    [TextArea(2, 4)] public string description;

    public string StateText
    {
        get
        {
            switch (currentState)
            {
                case State.Locked:
                    return price.ToString("#0.00") + " $";
                case State.Purchased:
                    return "Load";
                default:
                    return "Loaded";
            }
        }
    }
}
