using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CharacterInteractControl : MonoBehaviour
{
    public CharacterMovementModel myMoveModel;
    public List<Collider2D> ignoredColliders;
    [SerializeField]
    private float positionScale;
    public GameObject heldObject;
    private string heldObjName;
    // Start is called before the first frame update
    void Start()
    {
        myMoveModel = transform.parent.GetComponent<CharacterMovementModel>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteractPosition();
        if (Input.GetButtonDown("Fire1") && !AlienHealthController.Instance.playerControlsLocked)
            InteractWithZone();
    }

    /// <summary>
    /// Moves the position of the "interact zone" based on the
    /// direction the character is facing
    /// </summary>
    public void UpdateInteractPosition()
    {
        Vector2 facing = myMoveModel.GetDirection();
        if (facing == Vector2.zero)
            return;
        facing.Normalize();
        transform.localPosition = facing * positionScale;
    }

    /// <summary>
    /// Checks the contents of the zone trigger for interactable
    /// tiles, and triggers them, if available
    /// </summary>
    public void InteractWithZone()
    {
        Collider2D myCollider = GetComponent<Collider2D>();

        List<Collider2D> overlappingColliders = new List<Collider2D>();
        ContactFilter2D myFilter = new ContactFilter2D();
        myFilter.NoFilter();
        myCollider.OverlapCollider(myFilter, overlappingColliders);

        foreach (Collider2D contact in overlappingColliders)
        {
            //Debug.Log("Contacting: " + contact.name + "\n" + contact.GetType());
            if (ignoredColliders.Contains(contact))
            {
                //Debug.Log("IGNORING");
                continue;
            }
            if (contact.GetType() != typeof(TilemapCollider2D))
            {
                //Debug.Log("Found " + contact.name);
                if (contact.GetComponent<InteractableObject>() != null)
                    contact.GetComponent<InteractableObject>().InteractWithObject(this);
            }
        }
    }

    /// <summary>
    /// Attach a newly-spawned CarryableObject to this script
    /// </summary>
    /// <param name="attachedObj">The obj in question</param>
    public void AttachCarryableObject(string attachedName, Sprite attachedSprite)
    {
        heldObjName = attachedName;
        heldObject.GetComponent<SpriteRenderer>().sprite = attachedSprite;
        heldObject.SetActive(true);
    }

    /// <summary>
    /// When the player interacts with an OutputObject and is carrying something,
    /// the inputobject calls this method
    /// </summary>
    /// <param name="receivingObj">The object to transfer the carried obj to</param>
    public void GiveAttachedObj(InputObject receivingObj)
    {
        if (heldObjName == null)
            return;

        if (receivingObj.ReceiveHeldObject(heldObjName))
        {
            // If true, the giving was successful and we can clear our data
            heldObjName = null;
            heldObject.SetActive(false);
        }

    }

    public bool IsHoldingObject()
    {
        if (heldObjName == null)
            return false;
        return true;
    }
}
