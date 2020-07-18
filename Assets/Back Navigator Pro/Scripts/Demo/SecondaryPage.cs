using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BackNavigatorPro
{
    public class SecondaryPage : MonoBehaviour
    {
        public GameObject nextPage;

        public void OnClickBackButton()
        {
            gameObject.SetActive(false);
            nextPage.SetActive(true);
        }
    }
}