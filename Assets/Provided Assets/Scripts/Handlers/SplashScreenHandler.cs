using System.Collections;
using System.Collections.Generic;
using MeditativeBowls;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenHandler : MonoBehaviour
{
    public int sceneindex;
    void Start()
    {
       SceneManager.Instance.LoadScene(sceneindex);
    }
}
