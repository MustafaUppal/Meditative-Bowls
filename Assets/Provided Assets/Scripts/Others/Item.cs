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

    [SerializeField]private State currentState;
    public new string name;
    public string setName;
    public int set;
    public int Index;
    
    public float price;
    public Sprite image;
    [TextArea(2, 4)] public string description;

    public string StateText
    {
        get
        {
            switch (CurrentState)
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

    public State CurrentState
    {
        get => currentState;
        set
        {
            // Debug.Log("currentState: " + currentState + " -> " + value);
            currentState = value;
        }
    }
}
