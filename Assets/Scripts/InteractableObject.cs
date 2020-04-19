using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject selectedGlowPulse;
    public bool isAnimating = false;
    public List<AudioClip> myClips = new List<AudioClip>();
    /// <summary>
    /// Starts/Stops an interactible object glowing when player can interact with it
    /// </summary>
    public void DoGlowPulse(bool onOff)
    {
        if (selectedGlowPulse != null)
            selectedGlowPulse.SetActive(onOff);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterInteractControl interactible = collision.GetComponent<CharacterInteractControl>();
        if (interactible != null && !isAnimating)
            DoGlowPulse(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        CharacterInteractControl interactible = collision.GetComponent<CharacterInteractControl>();
        if (interactible != null)
            DoGlowPulse(false);
    }

    public virtual void InteractWithObject(CharacterInteractControl interactZone = null)
    {

    }

    public void PlaySoundFX(int num = 0)
    {
        var myAS = GetComponent<AudioSource>();
        if (myAS != null && myClips != null && myClips.Count > num && !myAS.isPlaying)
        {
            myAS.clip = myClips[num];
            myAS.Play();
        }
    }
}
