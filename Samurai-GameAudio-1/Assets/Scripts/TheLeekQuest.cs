using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLeekQuest : MonoBehaviour
{
    [Header("Triggers")]
    public GameObject leekTrigger;
    public GameObject marketSellerTrigger;

    [Header("UI Elements")]
    public GameObject pressToTalkUI;
    public GameObject pressToPickUpUI;
    public GameObject pressToGiveUI;
    public GameObject leekUIImage;
    [Header("Items")]
    public GameObject leekToCollect;
    public GameObject leekToGive;
    public bool questStarted;
    bool doesPlayerHaveLeek;
    [Header("FMOD")]
    public FMODUnity.EventReference acceptOrFinishQuestEvent, vegBoxPickup, leeksQuestDialogueStart, leeksQuestDialogueEnd;

    /*GAME AUDIO TIP
    Your FMOD & player objects should be defined here
    */

    void Start()
    {
        questStarted = false;
        pressToGiveUI.SetActive(false);
        pressToPickUpUI.SetActive(false);
        pressToTalkUI.SetActive(false);
        leekToGive.SetActive(false);
        leekUIImage.SetActive(false);
    }

    void Update()
    {
        LeekPickup();
        MarketSellerStart();
        MarketSellerEnd();
    }


    /* GAME AUDIO TIP
    MarketSellerStart starts The Leek Quest. 
    */

    void MarketSellerStart()
    {
        if (marketSellerTrigger.GetComponent<MarketSellerTrigger>().playerIsInMarketSellerTrigger == true && questStarted == false)
        {
            pressToTalkUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Dialogue should be called here
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("IsInQuest", 1f);
                FMODUnity.RuntimeManager.PlayOneShot(acceptOrFinishQuestEvent, transform.position);
                FMODUnity.RuntimeManager.PlayOneShot(leeksQuestDialogueStart, pressToTalkUI.transform.position);

                questStarted = true;
                pressToTalkUI.SetActive(false);
            }
        }
    }

    /* GAME AUDIO TIP
    Leek pickup (below) checks if the leek can be collected (if the quest has started) 
    and if the player is in the leek collection trigger box by referencing the 
    bool found in LeekPickup.cs
    */

    void LeekPickup()
    {
        if (questStarted == true && leekTrigger.GetComponent<LeekPickup>().playerIsInLeekTrigger == true)
        {
            if (doesPlayerHaveLeek == false)
            {
                pressToPickUpUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Pickup sound should be called here
                    //You could either call it at the players location or store the location of the box pickup
                    FMODUnity.RuntimeManager.PlayOneShot(vegBoxPickup, transform.position);

                    pressToPickUpUI.SetActive(false);
                    Destroy(leekToCollect);
                    doesPlayerHaveLeek = true;
                    leekUIImage.SetActive(true);


                }
            }
        }
    }

    /* GAME AUDIO TIP
    MarketSellerEnd checks if the player has the leeks and will allow the player
    to complete the quest and start the end quest dialogue
    */
    void MarketSellerEnd()
    {
        if (doesPlayerHaveLeek == true && marketSellerTrigger.GetComponent<MarketSellerTrigger>().playerIsInMarketSellerTrigger == true)
        {
            pressToGiveUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                //Dialogue should be called here
                FMODUnity.RuntimeManager.PlayOneShot(acceptOrFinishQuestEvent, transform.position);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("IsInQuest", 0f);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LeakQuestComplete", 1f);
                FMODUnity.RuntimeManager.PlayOneShot(leeksQuestDialogueEnd, pressToTalkUI.transform.position);

                doesPlayerHaveLeek = false;
                pressToGiveUI.SetActive(false);
                leekToGive.SetActive(true);
                leekUIImage.SetActive(false);
            }
        }
    }


}
