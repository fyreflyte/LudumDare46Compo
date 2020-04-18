using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUpObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " - " + gameObject.name);
        //LegoBrick collisionBrick = collision.GetComponent<LegoBrick>();
        //if (collisionBrick != null)
        //    collisionBrick.PickMeUp();
    }
}
