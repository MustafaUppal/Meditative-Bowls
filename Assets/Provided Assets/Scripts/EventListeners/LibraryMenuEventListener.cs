using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryMenuEventListener : MonoBehaviour
{
    public GameObject guidelineText;
    public GameObject sessionTile;
    public Transform tilesContainer;

    public Text Footertext;

    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }
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
        AllRefs.I.objectSelection.EnableClick(false);
    }

    public void OnClickBackButton()
    {
        // MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    public void LoadAllSessions()
    {
        SessionData sessionData = SessionManager.Instance.SessionData;
        int j = 0;

        for (int i = 0; i < tilesContainer.childCount; i++, j++)
        {
            if (i < sessionData.Length)
            {
                SessionData.Snipt session = SessionManager.Instance.SessionData.GetSession(i);

                bool havePositions = session.bowlsPositions.Length > 0;
                bool haveRecoding = session.recording != null;
                bool haveMP3 = false;
                Debug.Log("haveRecoding: " + haveRecoding);

                tilesContainer.GetChild(i).GetComponent<LibraryTileHandler>().SetTile
                (
                    session.name, new bool[3] { havePositions, haveRecoding, true }
                );
                tilesContainer.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (!tilesContainer.GetChild(i).gameObject.activeInHierarchy)
                    break;

                tilesContainer.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = j; i < sessionData.Length; i++)
        {
            SessionData.Snipt session = SessionManager.Instance.SessionData.GetSession(i);
            bool havePositions = session.bowlsPositions.Length > 0;
            bool haveRecoding = session.recording != null;
            bool haveMP3 = false;
            Debug.Log("haveRecoding: " + haveRecoding);
            Instantiate(sessionTile, tilesContainer).GetComponent<LibraryTileHandler>().SetTile
            (
                session.name, new bool[3] { havePositions, haveRecoding, true }
            ).SetActive(true);
        }

        // Refresh content
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        guidelineText.SetActive(sessionData.Length <= 0);
    }
}
