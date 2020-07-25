using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace BackNavigatorPro
{
    #if UNITY_EDITOR
    [HideScriptField]
    #endif
    public class BackNavigationHandler : MonoBehaviour
    {
        [Tooltip("Unique ID of every page, assigned automatically.")]
        #if UNITY_EDITOR
        [ReadOnly] 
        #endif
        [SerializeField] private int pageID;
        int savedID = -1;

        public int PageID
        {
            get => pageID;
            set
            {
                pageID = value;

                if (savedID.Equals(-1))
                    savedID = pageID;
            }
        }

        public UnityEvent OnEscapePressed;

        //private void OnValidate()
        //{
        //   pageID = savedID;
        //}

        private void Reset()
        {
            PageID = PageSequenceHandler.CurrentID++;
        }

        private void OnEnable()
        {
            PageSequenceHandler.AddPageID(PageID);
        }

        private void OnDisable()
        {
            PageSequenceHandler.RemovePageID(PageID);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && PageSequenceHandler.IsActivePage(PageID))
            {
                if (OnEscapePressed != null)
                    OnEscapePressed.Invoke();
            }
        }
    }
}