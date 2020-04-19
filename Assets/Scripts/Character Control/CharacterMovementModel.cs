using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementModel : MonoBehaviour
{
    public float characterSpeed;
    private Vector2 movementDirection;
    private Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        //Debug.Log("Update movement");
        if (movementDirection == Vector2.zero || AlienHealthController.Instance.playerControlsLocked)
            return;
        movementDirection.Normalize();

        myRigidBody.MovePosition((Vector2)transform.position + (movementDirection * characterSpeed * Time.deltaTime));
    }

    public void SetDirection(Vector2 direction)
    {
        movementDirection = direction;
    }

    public Vector2 GetDirection()
    {
        return movementDirection;
    }
    
    public bool IsMoving()
    {
        return movementDirection != Vector2.zero;
    }
}
