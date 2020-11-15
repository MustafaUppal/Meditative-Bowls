using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource audioSource;
    Coroutine playC;
    public int prevIndex = -1;

    public void Play(bool play, AudioClip clip = null, float time = -1, int index = -1)
    {
        if (prevIndex != -1)
        {
            AllRefs.I.tilesContainer.GetTile(prevIndex).buttonOnOff.SetIcon(false);
            AllRefs.I.tilesContainer.GetTile(prevIndex).playSound = false;
        }

        if (index != -1)
        {
            AllRefs.I.tilesContainer.GetTile(index).buttonOnOff.SetIcon(play);
            AllRefs.I.tilesContainer.GetTile(index).playSound = play;
        }


        prevIndex = index;
        if (playC != null)
            StopCoroutine(playC);

        if (play)
        {
            audioSource.clip = clip;
            audioSource.Play();

            if (!time.Equals(-1))
                playC = StartCoroutine(PlayE(time, index));
        }
        else
            audioSource.Stop();
    }

    IEnumerator PlayE(float time, int index)
    {
        yield return new WaitForSeconds(time);
        audioSource.Stop();
        AllRefs.I.tilesContainer.GetTile(index).buttonOnOff.SetIcon(false);
        AllRefs.I.tilesContainer.GetTile(prevIndex).playSound = false;

        playC = null;
    }
}