using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLightSync : MonoBehaviour
{
    AudioLoudness audioLoudness;
    public float intensityMultiplier;

    [Header("Required Range")]
    public float reqMin = 0;
    public float reqMax = 25;
    // new range value will be intensity of the light

    [Header("Given Range")]
    public float peakVal; // will be calculated on runtime
    public float bottomVal = 0;
    public float currentValue;

    [Space]
    public bool alowChangeColor;
    public float minimumColorChangeTime;
    public Color[] randomColors;

    [HideInInspector] public bool emit;
    new Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        audioLoudness = transform.parent.GetComponent<AudioLoudness>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((GameManager.Instance.State1 == GameManager.State.Randomization || GameManager.Instance.State1 ==
           GameManager.State.Normal || GameManager.Instance.State1 == GameManager.State.RepositionState
            || GameManager.Instance.State1 == GameManager.State.Sound && emit))
        {

            currentValue = audioLoudness.clipLoudness * intensityMultiplier;

            if (peakVal < currentValue)
                peakVal = currentValue;

            light.intensity = ConvertRange(bottomVal, peakVal, reqMin, reqMax, currentValue);

            // light.intensity = Mathf.Clamp(light.intensity, 1, 50) / 2;
            // if (maxValue < light.intensity)
            //     maxValue = light.intensity;

            if (alowChangeColor)
            {
                alowChangeColor = false;
                if (randomColors.Length > 0)
                    this.light.color = randomColors[Random.Range(0, randomColors.Length)];
                StartCoroutine(ChangeColorDelay());
            }
        }

        IEnumerator ChangeColorDelay()
        {
            yield return new WaitForSeconds(minimumColorChangeTime);
            alowChangeColor = true;
        }
    }

    public float ConvertRange(float oldMin, float oldMax, float newMin, float newMax, float oldVal)
    {
        float newVal = 0;

        float newRange = 0;
        float oldRange = (oldMax - oldMin);

        if (oldRange == 0)
            newVal = newMin;
        else
        {
            newRange = (newMax - newMin);
            newVal = (((oldVal - oldMin) * newRange) / oldRange) + newMin;
        }

        return newVal;
    }
}
