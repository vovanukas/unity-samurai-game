using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordQuest : MonoBehaviour
{
    public GameObject emperorsSword, emperorsSwordEnd, pickupTrigger, pickupInventory, endquestTrigger;

    [Header("UI Objects")]
    public GameObject pickupUI;
    public GameObject giveUI; 
    public GameObject dontHaveSwordUI;

    bool questComplete;

    [Header("Bools")]
    [SerializeField]
    bool playerHasEnteredSwordTrigger;
    [SerializeField]
    bool playerHasSword;

    /*GAME AUDIO TIP
    Your FMOD & player objects should be defined here
    */

    void Start()
    {
        playerHasSword = false;
        pickupUI.SetActive(false);
        giveUI.SetActive(false);
        dontHaveSwordUI.SetActive(false);
        pickupInventory.SetActive(false);
        questComplete = false;
        emperorsSwordEnd.SetActive(false);
    }

    void Update()
    {
        DisplayUI();
        PickupSword();
        EndQuest();
    }

    void PickupSword()
    {
        if (pickupTrigger.GetComponent<SwordPickup>().playerCanPickupSword == true && Input.GetKeyDown(KeyCode.E))
        {

            //Pickup sound should be called here
             //You could either call it at the players location or store the location of the sword pickup
            Destroy(emperorsSword);
            playerHasSword = true;
            pickupUI.SetActive(false);
            pickupInventory.SetActive(true);
        }

    }
    void EndQuest()
    {
        if (endquestTrigger.GetComponent<EndQuestTrigger>().playerInEndTrigger == true && playerHasSword && Input.GetKeyDown(KeyCode.E))
        {

            //Dialogue should be called here

            questComplete = true;
            emperorsSwordEnd.SetActive(true);
            pickupUI.SetActive(false);
            playerHasSword = false;
            pickupInventory.SetActive(false);
        }
    }

    #region UI
    void DisplayUI()
    {
        //UI for Pickup
        if (pickupTrigger.GetComponent<SwordPickup>().playerCanPickupSword == true && !playerHasSword)
        {
            pickupUI.SetActive(true);
        }
        else if (pickupTrigger.GetComponent<SwordPickup>().playerCanPickupSword == false && !playerHasSword)
        {
            pickupUI.SetActive(false);
        }

        //UI for End
        if (endquestTrigger.GetComponent<EndQuestTrigger>().playerInEndTrigger == true && playerHasSword && !questComplete)
        {
            giveUI.SetActive(true);
            dontHaveSwordUI.SetActive(false);

        }
        else if (endquestTrigger.GetComponent<EndQuestTrigger>().playerInEndTrigger == true && !playerHasSword && !questComplete)
        {
            giveUI.SetActive(false);
            dontHaveSwordUI.SetActive(true);
        }
        else if (endquestTrigger.GetComponent<EndQuestTrigger>().playerInEndTrigger == false)
        {
            giveUI.SetActive(false);
            dontHaveSwordUI.SetActive(false);
        }

        //Hide all UI when complete
        if (questComplete == true)
        {
            dontHaveSwordUI.SetActive(false);
            giveUI.SetActive(false);
            pickupUI.SetActive(false);
            
        }
        
    }
    #endregion
}
