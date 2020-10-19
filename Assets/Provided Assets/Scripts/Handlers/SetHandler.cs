using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHandler : MonoBehaviour
{
    public bool[] childrenInUse = { false, false, false, false, false, false, false, false };
    public bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickSetButton()
    {
        isOpened = !isOpened;

        int i = 1;
        foreach (bool childInUse in childrenInUse)
        {
            transform.GetChild(i++).gameObject.SetActive(isOpened ? childInUse : false);
        }

        Refresh();
    }

    public void Refresh()
    {
        AllRefs.I.tilesContainer.gameObject.SetActive(false);
        AllRefs.I.tilesContainer.gameObject.SetActive(true);
    }
}
