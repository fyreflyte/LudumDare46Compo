using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseWindowScript : MonoBehaviour
{
    public Animator mainTextAnimator;
    public Animator additionalAnimator;
    public Animator tryAgainAnimator;
    public Animator storyTextAnimator;
    public float fadeTitleText = 1;
    public float fadeButtonText = 1;
    public float fadeStoryText = 1;

    void OnEnable()
    {
        Invoke("FadeInMainText", fadeTitleText);
        Invoke("FadeInButton", fadeButtonText);
        Invoke("FadeInStory", fadeStoryText);
    }

    private void OnDisable()
    {
        mainTextAnimator.SetBool("fadeInObject", false);
        if (additionalAnimator != null)
            additionalAnimator.SetBool("fadeInObject", false);

        tryAgainAnimator.SetBool("fadeInObject", false);
        storyTextAnimator.SetBool("fadeInObject", false);
    }


    private void FadeInMainText()
    {
        mainTextAnimator.speed = 0.5f;
        mainTextAnimator.SetBool("fadeInObject", true);
        if (additionalAnimator != null)
            additionalAnimator.SetBool("fadeInObject", true);
    }

    private void FadeInButton()
    {
        tryAgainAnimator.SetBool("fadeInObject", true);
    }

    private void FadeInStory()
    {
        storyTextAnimator.speed = 0.3f;
        storyTextAnimator.SetBool("fadeInObject", true);
    }
}
