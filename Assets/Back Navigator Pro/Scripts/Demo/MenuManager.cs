using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BackNavigatorPro
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

        public enum MenuStates
        {
            Screen1,
            Screen2,
            Screen3
        }

        public MenuStates defaultState;
        public MenuStates currentState;
        public MenuStates prevState;

        public GameObject[] allScreens;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ChangeState(defaultState);
        }

        public void ChangeState(MenuStates newState)
        {
            prevState = currentState;
            currentState = newState;

            allScreens[(int)prevState].SetActive(false);
            allScreens[(int)currentState].SetActive(true);
        }
    }
}
