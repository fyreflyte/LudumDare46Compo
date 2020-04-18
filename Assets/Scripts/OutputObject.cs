using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object, when interacted with by the player, gives
/// out an item that the player will then carry around
/// </summary>
public class OutputObject : InteractableObject
{
    public string objGivenName;
    public Sprite objGivenSprite;
    public float cooldownTime = 0;
    public float cooldownCounter = 0;

    public override void InteractWithObject(CharacterInteractControl interactZone)
    {
        // If player is holding an object, return
        if (interactZone.IsHoldingObject())
            return;
        // If cooldown is not ready, return
        if (cooldownCounter - Time.time > cooldownTime)
            return;

        interactZone.AttachCarryableObject(objGivenName, objGivenSprite);
        PlaySoundFX();
        // Set my cooldown counter
        cooldownCounter = Time.time + cooldownTime;
    }
}