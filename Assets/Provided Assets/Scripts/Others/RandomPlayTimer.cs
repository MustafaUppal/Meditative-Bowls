using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomPlayTimer : MonoBehaviour
{

    public Text timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void Set(float totalSeconds, float seconds)
    {
        float time = totalSeconds - seconds;

        int mins = (int)time / 60;
        int secs = (int)time - mins * 60;

        this.timer.text = mins + ":" + (secs < 10 ? "0" + secs : secs.ToString());
    }
}
