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
    public string setName;
    public int set;
    public int index = -1;

    public int Index
    {
        get
        {
            if (index == -1)
                index = int.Parse(gameObject.name.Split('(')[1].Trim(')')) - 1;

            return index;
        }
    }

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

    public bool IsPurchased
    {
        get
        {
            // Debug.Log("currentState: " + currentState + " -> " + value);

            return currentState != State.Locked;
        }
    }
}
