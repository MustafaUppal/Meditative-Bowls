using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryMenuEventListener : MonoBehaviour
{
    public GameObject sessionTile;

    public Transform tilesContainer;

    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            replayBG = false,
            changeCamera = false,
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);

        LoadAllSessions();
    }

    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    public void LoadAllSessions()
    {
        int totalSessions = SessionManager.Instance.SessionData.Length;
        int j = 0;

        for (int i = 0; i < tilesContainer.childCount; i++, j++)
        {
            if (i < totalSessions)
            {
                Session session = SessionManager.Instance.SessionData.Get(i);
                tilesContainer.GetChild(i).GetComponent<LibraryTileHandler>().SetTile
                (
                    session.name, new bool[4] { true, false, false, true }
                );
                tilesContainer.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (!tilesContainer.GetChild(i).gameObject.activeInHierarchy)
                    return;

                tilesContainer.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = j; i < totalSessions; i++)
        {
            Session session = SessionManager.Instance.SessionData.Get(i);
            Instantiate(sessionTile, tilesContainer).GetComponent<LibraryTileHandler>().SetTile
            (
                session.name, new bool[4] { true, false, false, true }
            );
        }
    }
}
