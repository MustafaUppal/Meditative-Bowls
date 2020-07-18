using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBBF
{
    public class MainPage : MonoBehaviour
    {
        public void OnClickBackButton()
        {
            Debug.Log("Main Page: Quiting Application...");
            Application.Quit();
        }
    }
}