using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementView : MonoBehaviour
{
    public Animator myAnimator;
    public CharacterMovementModel myMoveModel;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        Vector2 direction = myMoveModel.GetDirection();

        if (myAnimator != null)
        {
            if (direction != Vector2.zero)
            {
                myAnimator.SetFloat("DirectionX", direction.x);
                myAnimator.SetFloat("DirectionY", direction.y);
            }

            myAnimator.SetBool("isMoving", myMoveModel.IsMoving());
        }
    }

    public void ZachCheckForNearbyStereos()
    {
        ////Debug.Log("CHecking steroes - " + GameManager.Instance.allStereoObjects.Count);
        //if (!zachDancing)
        //    return;
        //// Check for nearby stereos
        //foreach (GameObject stereo in GameManager.Instance.allStereoObjects)
        //{
        //    //Debug.Log(stereo.name + " - ");
        //    float distance = Vector2.Distance(transform.position, stereo.transform.position);
        //    if (distance < 2)
        //        stereo.GetComponent<InteractableAudioObject>().ZachDancingNearby();
        //}
    }

    /// <summary>
    /// Adds the ability to shoot Zach's flame particles in
    /// the 4 cardinal directions while he is dancing
    /// </summary>
    /// <param name="direction">1 -> right, 2 -> up, 3 -> left, 4 -> down</param>
    public void ZachDanceThrowParticles(int direction)
    {
        //if (zachParticleThrow)
        //    return;
        //var zDancePSMain = zDanceParticles.main;
        //var particleVelocity = zDanceParticles.forceOverLifetime;
        //if (direction == 1)
        //{
        //    zDancePSMain.startColor = Color.yellow;
        //    particleVelocity.x = 10;
        //}
        //else if (direction == 2)
        //{
        //    zDancePSMain.startColor = Color.red;
        //    particleVelocity.y = 10;
        //}
        //else if (direction == 3)
        //{
        //    zDancePSMain.startColor = Color.magenta;
        //    particleVelocity.x = -10;
        //}
        //else
        //{
        //    zDancePSMain.startColor = Color.cyan;
        //    particleVelocity.y = -10;
        //}

        //Invoke("ZachDanceResetParticles", 0.3f);
    }
}