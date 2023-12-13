using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject canvas;
    public GameObject mainMenu;
    public FMODUnity.EventReference uiClickEvent;
    void Start()
    {
        canvas = gameObject.transform.GetChild(0).gameObject;

        canvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !mainMenu.activeInHierarchy)
        {
            FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent, transform.position);

            canvas.SetActive(!canvas.activeInHierarchy);

            Cursor.visible = (canvas.activeInHierarchy) ? true : false;
            Cursor.lockState = (canvas.activeInHierarchy) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}

