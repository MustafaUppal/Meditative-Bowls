using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public class TileHandler : MonoBehaviour
{
    public Image image;
    public new Text name;
    public Image stateIcon;
    public Image bg;
    public int index;

    [Header("Toggles")]
    public bool enableDock;
    public ToggleHandler toggle;
    public ButtonOnOffSettings buttonOnOff;

    bool playSound = false;

    private void Awake()
    {
        bg = GetComponent<Image>();
    }

    public bool Highlight
    {
        set
        {
            bg.color = value
            ? AllRefs.I.highlightSettings.tileHighlight :
            AllRefs.I.highlightSettings.tileNormal;
        }
    }

    bool isLoaded;
    public bool IsLoaded
    {
        get => isLoaded;
        set { isLoaded = value; }
    }

    public void SetTile(Sprite sprite, string name, int index, int iconIndex)
    {
        // image.sprite = sprite;
        this.name.text = name;
        this.index = index;

        if (enableDock)
            DockSettings(iconIndex);
        else if (iconIndex == 0 || iconIndex == 1)
        {
            this.stateIcon.enabled = true;
            this.stateIcon.sprite = AllRefs.I.tilesContainer.buttonIcons[iconIndex];
            this.stateIcon.color = AllRefs.I.tilesContainer.buttonColors[iconIndex];
        }
        else
            this.stateIcon.enabled = false;
    }

    void DockSettings(int iconIndex)
    {
        // Debug.Log("iconIndex: " + iconIndex);
        if (iconIndex == 1 || iconIndex == 2) // is purchased or loaded
        {
            // Then show dock
            this.stateIcon.enabled = false;
            toggle.gameObject.SetActive(true);
            toggle.SetValue(iconIndex == 2); // enable toggle if bowl is loaded
            toggle.isInteractable = iconIndex == 2; // disbale toggle if bowl is off and vice versa
        }
        // if locked
        else if (iconIndex == 0)
        {
            toggle.gameObject.SetActive(false);
            this.stateIcon.enabled = true;
            this.stateIcon.sprite = AllRefs.I.tilesContainer.buttonIcons[iconIndex];
            this.stateIcon.color = AllRefs.I.tilesContainer.buttonColors[iconIndex];
        }
    }

    public void OnClickPlayButton()
    {
        playSound = !playSound;
        buttonOnOff.SetIcon(playSound);

        float timeToPlay = InventoryManager.Instance.allBowls[index].CurrentState.Equals(Item.State.Locked) ? 3 : -1;
        AudioClip clip = InventoryManager.Instance.allBowls[index].AudioSource.clip;

        AllRefs.I.audioHandler.Play(playSound, clip, timeToPlay);
    }

    public void OnClickToggle(bool val)
    {
        AllRefs.I.bowlsPlacementHandler.OnClickOffBowl(index);
        toggle.SetValue(false); // disbale toggle 
        toggle.isInteractable = false; // disbale toggle
    }
}
