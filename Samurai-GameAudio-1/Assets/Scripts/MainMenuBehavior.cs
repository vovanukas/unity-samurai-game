using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [Header("FMOD Events")]
    public FMODUnity.EventReference mainMenuMusicEvent;
    public FMOD.Studio.EventInstance mainMenuMusicEventInstance;
    public FMODUnity.EventReference uiClickEvent;

    public Camera mainMenuCamera;
    public Canvas playerGUI;
    public Canvas[] menuScreens;

    public Animator animator1;
    public Animator animator2;

    public static bool showMainMenu = true;
    private static Camera[] _activeCameras;



    // private Animation mainMenuCameraAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if (showMainMenu)
        {
            animator1.Play("mixamo_com", -1);
            animator2.Play("mixamo_com", -1);

            mainMenuMusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(mainMenuMusicEvent);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(mainMenuMusicEventInstance, this.transform);
            mainMenuMusicEventInstance.start();

            Camera[] _activeCameras = Camera.allCameras;
            for (int i = 0; i < _activeCameras.Length; i++)
            {
                _activeCameras[i].enabled = false;
            }
            mainMenuCamera.enabled = true;

            playerGUI.gameObject.SetActive(false);

            GameObject.FindGameObjectWithTag("Player").gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);

            playerGUI.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // for (int i = 0; i < _activeCameras.Length; i++)
            // {
            //     _activeCameras[i].enabled = true;
            // }
        }
    }

    void Update()
    {
        if (showMainMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void OnStartButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent, transform.position);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("IsInMainMenu", 0f);
        showMainMenu = false;

        SceneManager.LoadScene(0);
    }

    public void OnCreditsButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent, transform.position);
        menuScreens[0].gameObject.SetActive(false);
        menuScreens[1].gameObject.SetActive(true);
    }

    public void OnQuitButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent, transform.position);
        Application.Quit();
    }

    public void OnCreditsBackButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClickEvent, transform.position);
        menuScreens[0].gameObject.SetActive(true);
        menuScreens[1].gameObject.SetActive(false);
    }
}
