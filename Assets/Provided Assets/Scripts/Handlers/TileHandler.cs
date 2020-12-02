using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightProperties
{
    Selected,
    Normal,
    Loaded
}

public class TileHandler : MonoBehaviour
{
    public Image image;
    public new Text name;
    public Image stateIcon;
    public Image bg;
    public int index;

    [Header("Toggles")]
    public bool enableDock;
    // public ToggleHandler toggle;
    public GameObject removeBtn;
    public ButtonOnOffSettings buttonOnOff;

    public bool playSound = false;

    private void Awake()
    {
        bg = GetComponent<Image>();
    }

    public HighlightProperties Highlight
    {
        set
        {
            switch (value)
            {
                case HighlightProperties.Loaded:
                    bg.color = AllRefs.I.highlightSettings.loaded;
                    break;
                case HighlightProperties.Normal:
                    bg.color = AllRefs.I.highlightSettings.normal;
                    break;
                case HighlightProperties.Selected:
                    bg.color = AllRefs.I.highlightSettings.selected;
                    break;
            }
        }
    }

    // bool isLoaded;
    // public bool IsLoaded
    // {
    //     get => isLoaded;
    //     set { isLoaded = value; }
    // }

    public Item.State currentState;

    public HighlightProperties GetNormal()
    {
        return !enableDock ? HighlightProperties.Normal : (currentState.Equals(Item.State.Loaded) ? HighlightProperties.Loaded : HighlightProperties.Normal);
    }
    public void SetTile(Sprite sprite, string name, int index)
    {
        // image.sprite = sprite;
        this.name.text = name;
        this.index = index;

        if (enableDock)
            DockSettings((int)currentState);
        else
        {
            this.stateIcon.enabled = true;
            this.stateIcon.sprite = AllRefs.I.tilesContainer.buttonIcons[(int)currentState];
            this.stateIcon.color = AllRefs.I.tilesContainer.buttonColors[(int)currentState];
        }
    }

    void DockSettings(int iconIndex)
    {
        // Debug.Log("iconIndex: " + iconIndex);
        if (iconIndex == 2) // if loaded
        {
            // Then show dock
            this.stateIcon.enabled = false;
            // toggle.gameObject.SetActive(true);
            // toggle.SetValue(iconIndex == 2); // enable toggle if bowl is loaded
            // toggle.isInteractable = iconIndex == 2; // disbale toggle if bowl is off and vice versa
            removeBtn.SetActive(true);
        }
        // if locked or purchased
        else if (iconIndex == 0 || iconIndex == 1)
        {
            // toggle.gameObject.SetActive(false);
            removeBtn.SetActive(false);

            this.stateIcon.enabled = true;
            this.stateIcon.sprite = AllRefs.I.tilesContainer.buttonIcons[iconIndex];
            this.stateIcon.color = AllRefs.I.tilesContainer.buttonColors[iconIndex];
        }
    }

    Coroutine coroutine;
    public void OnClickPlayButton()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(LoadBeforePlay());
    }

    IEnumerator LoadBeforePlay()
    {
        float timeToPlay = InventoryManager.Instance.allBowls[index].CurrentState.Equals(Item.State.Locked) ? 3 : -1;
        AudioClip clip = InventoryManager.Instance.allBowls[index].AudioSource.clip;

        // Do not play not purchased sounds in any state other than shop
        if (timeToPlay == 3 && !MenuManager.Instance.currentState.Equals(MenuManager.MenuStates.Shop))
        {
            StopCoroutine(coroutine);
        }

        playSound = !playSound;
        buttonOnOff.SetIcon(playSound);

        if (clip.loadState != AudioDataLoadState.Loaded)
        {
            PopupManager.Instance.spinnerLoading.Show("Loading Sound");
            // if previous audio is loaded then unload it
            int spareIndex = InventoryManager.Instance.bowlsManager.spareAudioIndex;
            if (spareIndex != -1 && InventoryManager.Instance.allBowls[spareIndex].AudioSource.clip.loadState == AudioDataLoadState.Loaded)
            {
                yield return InventoryManager.Instance.allBowls[spareIndex].AudioSource.clip.UnloadAudioData();
            }

            // load new auido to play
            yield return InventoryManager.Instance.allBowls[index].AudioSource.clip.LoadAudioData();
            InventoryManager.Instance.bowlsManager.spareAudioIndex = index;
            PopupManager.Instance.spinnerLoading.Hide();
        }
        
        // Play
        AllRefs.I.audioHandler.Play(playSound, clip, timeToPlay, index);
    }

    public void OnClickToggle(bool val)
    {
        AllRefs.I.bowlsPlacementHandler.OnClickOffBowl(index);
        // toggle.SetValue(false); // disbale toggle 
        // toggle.isInteractable = false; // disbale toggle
    }
}
