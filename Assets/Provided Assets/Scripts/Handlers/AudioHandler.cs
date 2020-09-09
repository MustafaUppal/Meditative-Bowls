using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource audioSource;
    Coroutine playC;

    public void Play(bool play, AudioClip clip = null, float time = -1)
    {
        if (playC != null)
            StopCoroutine(playC);

        if (play)
        {
            audioSource.clip = clip;
            audioSource.Play();

            if (!time.Equals(-1))
                playC = StartCoroutine(PlayE(time));
        }
        else
            audioSource.Stop();
    }

    IEnumerator PlayE(float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.Stop();

        playC = null;
    }
}
