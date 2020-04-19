using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnerInput : InputObject
{
    public int cookTime = 10;
    public float cookTimeRemaining = 0;
    string cookingObject;
    bool cookingFinished;
    public Image timerBar;

    public GameObject currentItem;
    public GameObject fireParticlesAttachment;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cookTimeRemaining > 0)
        {
            timerBar.transform.parent.gameObject.SetActive(true);
            timerBar.fillAmount = cookTimeRemaining / cookTime;
            cookingFinished = false;
            cookTimeRemaining -= Time.deltaTime;
        }
        else
        {
            timerBar.transform.parent.gameObject.SetActive(false);
            if (cookingFinished == false)
            {
                cookingFinished = true;
                SetAttachedResourceAppearance(true);
            }
        }
    }

    public override void InteractWithObject(CharacterInteractControl interactZone)
    {
        // If empty && player has valid object, take object and begin cooking it
        if (cookingObject == null && cookTimeRemaining <= 0 && interactZone.IsHoldingObject())
        {
            interactZone.GiveAttachedObj(this);
            //PlaySoundFX(0);
            return;
        }

        // If full and cooking completed, and player is empty, give result to player
        if (cookingObject != null && cookTimeRemaining <= 0 && !interactZone.IsHoldingObject())
        {
            interactZone.AttachCarryableObject(QueryCookedObject());
            cookingObject = null;
            currentItem.SetActive(false);
            PlaySoundFX(1);
            return;
        }
        
    }

    /// <summary>
    /// Called by InputObject - used here to begin cooking
    /// </summary>
    /// <param name="objName">Name of the resource given</param>
    public override void ExtraReceive(string objName)
    {
        base.ExtraReceive(objName);
        cookingObject = objName;
        cookTimeRemaining = cookTime;

        SetAttachedResourceAppearance(false);
    }

    public string QueryCookedObject()
    {
        if (cookingObject == null)
            return "Nothing";

        switch (cookingObject)
        {
            case "Raw Meat":
                return "Cooked Meat";
            case "Sodium":
                return "Fire";
            default:
                return "Unknown";
        }
    }

    public void SetAttachedResourceAppearance(bool isCookedMat)
    {
        string newAttachedResource;
        if (isCookedMat)
        {
            newAttachedResource = QueryCookedObject();
        }
        else
            newAttachedResource = cookingObject;

        currentItem.SetActive(true);
        fireParticlesAttachment.SetActive(newAttachedResource == "Fire"); // If this is the fire plate, turn on the particles
        currentItem.GetComponent<SpriteRenderer>().sprite = HealthBarAttributeController.Instance.GetResourceSprite(newAttachedResource);
    }
}
