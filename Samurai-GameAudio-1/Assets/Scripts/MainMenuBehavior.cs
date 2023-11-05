using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    public Camera mainMenuCamera;
    public Canvas playerGUI;
    public Canvas[] menuScreens;

    public static bool showMainMenu = true;
    private static Camera[] _activeCameras;



    // private Animation mainMenuCameraAnimation;

    // Start is called before the first frame update
    void Start()
    {

        if (showMainMenu)
        {
            Camera[] _activeCameras = Camera.allCameras;
            for (int i = 0; i < _activeCameras.Length; i++)
            {
                _activeCameras[i].enabled = false;
            }
            mainMenuCamera.enabled = true;

            playerGUI.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);

            playerGUI.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            for (int i = 0; i < _activeCameras.Length; i++)
            {
                _activeCameras[i].enabled = true;
            }
        }
    }

    void Update()
    {
        if(showMainMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void OnStartButton()
    {
        showMainMenu = false;
        SceneManager.LoadScene(0);
    }

    public void OnCreditsButton()
    {
        menuScreens[0].gameObject.SetActive(false);
        menuScreens[1].gameObject.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnCreditsBackButton()
    {
        menuScreens[0].gameObject.SetActive(true);
        menuScreens[1].gameObject.SetActive(false);
    }
}
