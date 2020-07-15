using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLightSync : MonoBehaviour
{
    public bool alowChangeColor;
    public float minimumColorChangeTime;
    public Color[] randomColors;

    public AudioSpectrum audioSpectrum;
    public bool emit;

    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.state == GameManager.State.Normal&&emit)
        {
            light.intensity = 1 + AudioSpectrum.spectrumValue * 1;
            light.intensity = Mathf.Clamp(light.intensity, 1, 50);

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
