using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base movement model for all characters (both player and AI-controlled)
/// </summary>
public class CharacterBaseControl : MonoBehaviour
{
    protected CharacterMovementModel movementModel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void SetDirection(Vector2 direction)
    {
        movementModel.SetDirection(direction);
    }
}