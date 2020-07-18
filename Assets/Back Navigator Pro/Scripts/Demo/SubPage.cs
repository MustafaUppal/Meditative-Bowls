using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BackNavigatorPro
{
    public class SubPage : MonoBehaviour
    {
        public void OnClickBackButton()
        {
            gameObject.SetActive(false);
        }
    }
}
