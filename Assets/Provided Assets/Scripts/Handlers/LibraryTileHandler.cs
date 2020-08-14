﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryTileHandler : MonoBehaviour
{
    public new Text name;
    public Transform container;

    public void SetTile(string name, bool[] activeStates)
    {
        
        this.name.text = name;

        for (int i = 0; i < container.childCount; i++)
        {
            container.GetChild(i).gameObject.SetActive(activeStates[i]);
        }
    }

    public void OnClickLoadButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        SessionManager.Instance.LoadSession(name.text);
    }

    public void OnClickPlayRecordingButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        SessionManager.Instance.LoadSession(name.text, true);
        // TODO: playrecording
    }

    public void OnClickPlayMp3Button()
    {
       
    }

    public void OnClickDeleteButton()
    {
        SessionManager.Instance.DeleteSession(name.text);
        gameObject.SetActive(false);
        transform.SetParent(AllRefs.I.libraryMenu.tilesContainer);
    }
}
