using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerControl : CharacterBaseControl
{
    // Start is called before the first frame update
    void Awake()
    {
        movementModel = GetComponent<CharacterMovementModel>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
    }

    void UpdateDirection()
    {
        //if (GameManager.Instance.inputLocked)
        //    return;
        
        //Vector2 newDirection = new Vector2(touchControlJoystick.Horizontal, touchControlJoystick.Vertical);
        var horiz = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");

        Vector2 newDirection = new Vector2(horiz, vert);
        if (!AlienHealthController.Instance.playerControlsLocked)
            SetDirection(newDirection);
    }
}
