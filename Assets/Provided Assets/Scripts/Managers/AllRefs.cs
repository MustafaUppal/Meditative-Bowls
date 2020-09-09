using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;

public class AllRefs : MonoBehaviour
{
    public static AllRefs I;

    private void Awake()
    {
        I = this;
    }
    
    [Header("Menus")]
    public MainMenuEventListener mainMenu;
    public DockEventListener dock;
    public ShopMenuEventListener shopMenu;
    public LibraryMenuEventListener libraryMenu;
    public RecordingMenuEventListener recordingMenu;
    public HeaderHandler headerHandler;
    public SettingsMenuEventListener settingMenu;
    public GameManager _GameManager;
    [Header("Objects")]
    public GameObject itemContainer;
    public HighlightSettings highlightSettings;
}
