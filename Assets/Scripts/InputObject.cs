using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These objects receive a player's carried object
/// </summary>
public class InputObject : InteractableObject
{
    private float cooldownTime = 0;
    private float cooldownCounter = 0;
    public List<string> validReceivableObjects = new List<string>();
    public override void InteractWithObject(CharacterInteractControl obj)
    {
        if (cooldownCounter - Time.time > cooldownTime)
        {
            // TODO: Play rejected sound
            return;
        }

        obj.GiveAttachedObj(this);
    }

    /// <summary>
    /// Receives an object being held by the player
    /// </summary>
    /// <param name="objName"></param>
    public bool ReceiveHeldObject(string objName)
    {
        if (!validReceivableObjects.Contains(objName) && !validReceivableObjects.Contains("All"))
            return false;

        // Set my cooldown counter
        cooldownCounter = Time.time + cooldownTime;
        PlaySoundFX();
        return true;
    }
}
