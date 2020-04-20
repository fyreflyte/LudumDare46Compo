using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixerInput : InputObject
{
    public int mixTime = 10;
    public float mixTimeRemaining = 0;
    string mixObject1;
    string mixObject2;

    bool mixingFinished;
    public Image timerBar;

    public GameObject spinnerArm;
    public GameObject currentItem1;
    public GameObject fireParticlesAttachment;
    public GameObject currentItem2;

    private float rotationSpeed = 1;
    public float MAX_SPEED = 50;
    private float mixSpeedDelta = 1;

    private List<string> savedValidReceivableObjects = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (string str in validReceivableObjects)
            savedValidReceivableObjects.Add(str);
    }

    // Update is called once per frame
    void Update()
    {
        if (mixTimeRemaining > 0)
        {
            mixSpeedDelta = mixTimeRemaining / mixTime < 0.5f ? -1 : 1;
            RotateMixerArm();
            timerBar.transform.parent.gameObject.SetActive(true);
            timerBar.fillAmount = mixTimeRemaining / mixTime;
            mixingFinished = false;
            mixTimeRemaining -= Time.deltaTime;
        }
        else
        {
            timerBar.transform.parent.gameObject.SetActive(false);
            if (mixingFinished == false)
            {
                mixingFinished = true;
                SetAttachedResourceAppearance(3);
                validReceivableObjects = new List<string>(savedValidReceivableObjects);
                spinnerArm.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }

    public void RotateMixerArm()
    {
        // Spin arm
        var rotationVector = spinnerArm.transform.rotation.eulerAngles;
        rotationVector.z += rotationSpeed;
        spinnerArm.transform.rotation = Quaternion.Euler(rotationVector);
        if (rotationSpeed < MAX_SPEED && rotationSpeed >= 1)
            rotationSpeed += mixSpeedDelta;
    }

    public override void InteractWithObject(CharacterInteractControl interactZone)
    {
        // If empty && player has valid object, take object
        if (mixObject1 == null  && mixTimeRemaining <= 0 && interactZone.IsHoldingObject())
        {
            interactZone.GiveAttachedObj(this);
            return;
        }
        if (mixObject1 != null && mixObject2 == null && mixTimeRemaining <= 0 && interactZone.IsHoldingObject())
        {
            interactZone.GiveAttachedObj(this);
            return;
        }


        // If full and cooking completed, and player is empty, give result to player
        if (mixObject1 != null && mixTimeRemaining <= 0 && !interactZone.IsHoldingObject())
        {
            interactZone.AttachCarryableObject(QueryMixedObject());
            mixObject1 = null;
            mixObject2 = null;
            currentItem1.SetActive(false);
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
        if (mixObject1 == null)
        {
            mixObject1 = objName;
            validReceivableObjects = new List<string>() { QueryPairedResource() };
            SetAttachedResourceAppearance(1);
            return;
        }

        if (mixObject2 == null)
        {
            mixObject2 = objName;
            SetAttachedResourceAppearance(2);
            mixTimeRemaining = mixTime;
            return;
        }
    }

    public string QueryPairedResource()
    {
        if (mixObject1 == null)
            return "Nothing";

        switch (mixObject1)
        {
            case "Chlorine":
                return "Sodium";
            case "Sodium":
                return "Chlorine";
            case "Iron":
                return "Sulphur";
            case "Sulphur":
                return "Iron";
            default:
                return "Unknown";
        }
    }

    public string QueryMixedObject()
    {
        if (mixObject1 == null)
            return "Nothing";

        switch (mixObject1)
        {
            case "Chlorine":
                return "Salt";
            case "Sodium":
                return "Salt";
            case "Iron":
                return "Fire";
            case "Sulphur":
                return "Fire";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// Set appearance of items in mixer
    /// </summary>
    /// <param name="mixNum">1: first item, 2: second item, 3: mixed item</param>
    public void SetAttachedResourceAppearance(int mixNum)
    {
        if (mixNum == 1)
        {
            currentItem1.SetActive(true);
            currentItem1.GetComponent<SpriteRenderer>().sprite = HealthBarAttributeController.Instance.GetResourceSprite(mixObject1);
            currentItem2.SetActive(false);
        }
        else if (mixNum == 2)
        {
            currentItem2.SetActive(true);
            currentItem2.GetComponent<SpriteRenderer>().sprite = HealthBarAttributeController.Instance.GetResourceSprite(mixObject2);
        }
        else if (mixNum == 3)
        {
            currentItem1.SetActive(true);
            currentItem1.GetComponent<SpriteRenderer>().sprite = HealthBarAttributeController.Instance.GetResourceSprite(QueryMixedObject());
            currentItem2.SetActive(false);
            fireParticlesAttachment.SetActive(QueryMixedObject() == "Fire"); // If this is the fire plate, turn on the particles
        }
    }
}
