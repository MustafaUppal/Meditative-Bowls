using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLightSync : MonoBehaviour
{
    AudioLoudness audioLoudness;
    public float intensityMultiplier;
    
    public float maxValue;

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

        if (GameManager.Instance.state == GameManager.State.Normal ||GameManager.Instance.state==GameManager.State.Sound&& emit)
        {
            light.intensity = audioLoudness.clipLoudness * intensityMultiplier;
            light.intensity = Mathf.Clamp(light.intensity, 1, 50);

            if(maxValue < light.intensity)
                maxValue = light.intensity;

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
}
