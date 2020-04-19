////////////////////////////////////////////////////////////////////////////////////////
//        Copyright Jason Woerner 2018                                                //
////////////////////////////////////////////////////////////////////////////////////////
//
// Generic script that creates "floating" text
// Typically used to denote damage or completion of tasks

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour
{
    public static FloatingTextController Instance;

    public Transform mainCanvas;
    public GameObject floatingTextPrefab;   // Prefab must contain a Text or TextMeshProUGUI component
    public float globalSpeedMult = 0.01f;
    public bool floatingTextDisabled = false;

    // Runs before Start()
    void Awake()
    {
        Instance = this;
    }

    // TODO: Create individual prefabs for each type of floatign text (damage, healing, etc) and add them to an array
    //       Specify the array number when requesting floating text - this will allow for much more stylistic displays
    public FloatingPopupText CreateFloatingPopupText(string text, Vector2 location, Color color = new Color(), FloatingFadeMethod method = FloatingFadeMethod.None, float speedMult = 1, float randomXRange = 0, float randomSpeedMult = 1, float textTime = 2, float maxPlaneDistance = 0)
    {
        if (floatingTextDisabled)
            return null;
        GameObject newFloatingText = Instantiate(floatingTextPrefab);
        FloatingPopupText popupText = newFloatingText.GetComponent<FloatingPopupText>();
        popupText.StartFloating(text, globalSpeedMult, randomXRange, randomSpeedMult, textTime, maxPlaneDistance);
        newFloatingText.transform.position = location;
        newFloatingText.transform.SetParent(mainCanvas, false);
        if (method != FloatingFadeMethod.None)
            newFloatingText.GetComponent<FloatingPopupText>().SelectFadeMethod(method);
        if (color != new Color())
            popupText.proFloatingText.color = color;

        // Add a small delay to further popups
        floatingTextDisabled = true;
        Invoke("EnableFloatingText", 1);
        return popupText;
    }

    private void EnableFloatingText()
    {
        floatingTextDisabled = false;
    }

    /// <summary>
    /// Adds an image to the floating text, either left or right of it
    /// Currently supports 1 left and 1 right image, but not multiples of each
    /// The image can be resized by passing a custom size
    /// </summary>
    /// <param name="isPrefix">If true, the image appears to the left of the text</param>
    /// <param name="image">The sprite you wish to display</param>
    /// <param name="imageSize">A Vector2 containing a custom width and height for the sprite</param>
    public void SetFloatingImage(FloatingPopupText popupText, bool isPrefix, Sprite image, Vector2 imageSize = new Vector2())
    {
        Image workingImage = popupText.prefixImage;
        if (!isPrefix)
            workingImage = popupText.suffixImage;

        float width = imageSize.x;
        float height = imageSize.y;
        if (width > 0 && height > 0)
        {
            workingImage.GetComponent<LayoutElement>().preferredWidth = width;
            workingImage.GetComponent<LayoutElement>().preferredHeight = height;
        }
        workingImage.sprite = image;
    }

    // Example - Enable to test floating text
    public void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //    CreateFloatingPopupText("Damage: " + Random.Range(1, 51), Camera.main.ScreenToWorldPoint(Input.mousePosition), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }
}
