using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuButtons : MonoBehaviour
{
    public FMODUnity.EventReference uiClickEvent;
    public GameObject canvas;

    GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void OnQuitGameButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent);
        Application.Quit();
    }

    public void OnExitMenuButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent);

        canvas.SetActive(!canvas.activeInHierarchy);

        Cursor.visible = (canvas.activeInHierarchy) ? true : false;
        Cursor.lockState = (canvas.activeInHierarchy) ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
