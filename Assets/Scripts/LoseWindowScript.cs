using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseWindowScript : MonoBehaviour
{
    public Animator gameOverAnimator;
    public Animator tryAgainAnimator;

    void OnEnable()
    {
        gameOverAnimator.SetBool("fadeInObject", true);
        Invoke("FadeInButton", 1);
    }

    private void OnDisable()
    {
        gameOverAnimator.SetBool("fadeInObject", false);
        tryAgainAnimator.SetBool("fadeInObject", false);
    }

    private void FadeInButton()
    {

        tryAgainAnimator.SetBool("fadeInObject", true);
    }
}
