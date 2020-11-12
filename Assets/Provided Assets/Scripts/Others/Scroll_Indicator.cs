using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll_Indicator : MonoBehaviour
{
    public GameObject uperArrow;
    public GameObject lowerArrow;

    public float upperArrowRange = 0.99f;
    public float LowerArrowRange = 0.01f;

    ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("srv: " + scrollRect.verticalNormalizedPosition);;
        uperArrow.SetActive(scrollRect.verticalNormalizedPosition <= upperArrowRange);
        lowerArrow.SetActive(scrollRect.verticalNormalizedPosition >= LowerArrowRange);
    }
}
