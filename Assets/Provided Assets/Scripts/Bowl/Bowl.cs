using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bowl : Item
{

    // Variables
    public int position;
    public Color lightColor;
    public Material material;

    public MeshFilter mesh;

    public MeshFilter Mesh 
    {
        get
        {
            if(mesh == null)
                mesh = gameObject.GetComponent<MeshFilter>();

            return mesh;
        }
    }


    // Private Variables
    AudioSource audioSource;
    float panStereo = -1;
    float volume = -1;

    // Propertise
    public AudioSource AudioSource
    {
        get
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            return audioSource;
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        transform.GetChild(0).GetComponent<Light>().color = lightColor;
    }

    Coroutine coroutine;
    private void OnEnable()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(LoadBowSound());
    }

    IEnumerator LoadBowSound()
    {
        if (AudioSource.clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
            InventoryManager.Instance.bowlsManager.AddBowlLoading();
            yield return AudioSource.clip.LoadAudioData();
            InventoryManager.Instance.bowlsManager.RemoveBowlLoading();
        }
        coroutine = null;
    }

    private void OnDisable()
    {
        if (AudioSource.clip.loadState == AudioDataLoadState.Loaded)
            AudioSource.clip.UnloadAudioData();
    }

    private void Update()
    {
        if (audioSource)
            gameObject.transform.GetChild(0).gameObject.SetActive(audioSource.isPlaying);
    }

    public void PlaySound()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<AudioLightSync>().emit = true;

        if (!gameObject.activeInHierarchy) return;

        audioSource.Stop();
        audioSource.Play();

        InventoryManager.Instance.bowlsManager.AddPlayingAudio(base.Index, AudioSource);
    }
}
