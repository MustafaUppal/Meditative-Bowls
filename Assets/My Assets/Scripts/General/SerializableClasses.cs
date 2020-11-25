using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SerializableClasses
{
    [Serializable]
    public class LoadingSettings
    {
        public GameObject loading;
        [HideInInspector] public bool isLoading;
        public Coroutine loadingCoroutine;
    }

    [Serializable]
    public class ErrorSettings
    {
        public Animator errorPanel;

        public GameObject simpleButton;
        public GameObject confirmationButtons;

        [HideInInspector] public string message;
        public enum HideTypes
        {
            vanish,
            smooth
        }

        bool ConfirmationButtons
        {
            set
            {
                if (confirmationButtons != null)
                    confirmationButtons.SetActive(value);
            }
        }

        bool SimpleButton
        {
            set
            {
                if (simpleButton != null)
                    simpleButton.SetActive(value);
            }
        }

        public void Hide(HideTypes hidetype = HideTypes.vanish)
        {
            if (hidetype == HideTypes.vanish)
                errorPanel.gameObject.SetActive(false);
            else if (hidetype == HideTypes.smooth)
                errorPanel.SetBool("isActive", false);
        }

        public void Show(string errorMessage = "", bool isConfimationPanel = false)
        {
            if (errorMessage.Equals("") && message.Equals(""))
                return;

            errorMessage = errorMessage.Equals("") ? message : errorMessage;

            // Activating main panel
            errorPanel.gameObject.SetActive(true);
            errorPanel.SetBool("isActive", true);

            // Activating simple buttons and deactivating
            ConfirmationButtons = isConfimationPanel;
            SimpleButton = !isConfimationPanel;

            // Message to show
            errorPanel.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = errorMessage;
            message = "";
        }
    }

    [Serializable]
    public class PhotoPickerSettings : MonoBehaviour
    {

    }
}